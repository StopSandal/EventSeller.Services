using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.DataLayer.EntitiesViewModel;
using EventSeller.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EventSeller.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service class for user-related operations. Include Login, RefreshToken and CRUD operation. Also You can get UserRoles.
    /// </summary>
    public class UserService : IUserService
    {
        private const string LOGIN_PROVIDER = "Server";
        private const string TOKEN_NAME = "JWT";
        private const string USER_BASE_ROLE = "Basic";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IJWTFactory _JWTFactory;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IJWTFactory jWTFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _JWTFactory = jWTFactory;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the username or email is already in use.</exception>
        public async Task CreateUserAsync(AddUserDto addUserDto)
        {
            if(await GetUserByUserNameAsync(addUserDto.UserName) != null)
            {
                throw new InvalidOperationException("This UserName is already taken.");
            }
            if (await _userManager.FindByEmailAsync(addUserDto.Email) != null)
            {
                throw new InvalidOperationException("This Email is already in use.");
            }
            var user = new User
            {
                UserName = addUserDto.UserName,
                Email = addUserDto.Email
            };

            var result = await _userManager.CreateAsync(user,addUserDto.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Password doesn't meet requirements {result.Errors}");
            }
            await _userManager.AddToRoleAsync(user, USER_BASE_ROLE);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Can't assign basic user role {result.Errors}");
            }
        }
        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist or deletion fails.</exception>
        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) 
            {
                throw new InvalidOperationException("User doesn't exists");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Can't delete user {result.Errors}");
            }
        }
        /// <inheritdoc />
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when the user does not exist or the password is incorrect.</exception>
        public async Task<TokenVM> LoginAsync(LoginUserVM user)
        {
            var existingUser = await _userManager.Users
                                        .FirstOrDefaultAsync(u => u.UserName == user.UserName || u.Email == user.Email);
            if (existingUser == null)
            {
                throw new InvalidDataException("This user doesn't exists");
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(existingUser, user.Password, false);
            
            if(!result.Succeeded) 
            {
                throw new InvalidDataException("Password is wrong.");
            }

            // get claims and JWT token
            var claims = await GetUserClaimsAsync(existingUser);
            var token = _JWTFactory.GenerateToken(claims);
            //set JWT token
            await _userManager.RemoveAuthenticationTokenAsync(existingUser, LOGIN_PROVIDER, TOKEN_NAME);
            await _userManager.SetAuthenticationTokenAsync(existingUser, LOGIN_PROVIDER, TOKEN_NAME, token);
            //set Refresh token for user
            var refreshToken = _JWTFactory.GenerateRefreshToken();
            existingUser.RefreshToken = refreshToken;
            existingUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(_JWTFactory.GetRefreshTokenValidityInDays());
            await _userManager.UpdateAsync(existingUser);

            return new TokenVM()
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }
        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the access token or refresh token is invalid.</exception>
        public async Task<TokenVM> RefreshTokenAsync(TokenVM token)
        {
            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var identity = await _JWTFactory.GetIdentityFromExpiredTokenAsync(accessToken);

            if (identity == null)
            {
                throw new InvalidOperationException("Invalid access token");
            }

            string username = identity.Name;

            var user = await GetUserByUserNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new InvalidOperationException("Invalid access token or refresh token");
            }

            // set new refresh token
            var newRefreshToken = _JWTFactory.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            // reset JWT token
            var newAccessToken = _JWTFactory.GenerateToken(await GetUserClaimsAsync(user));
            await _userManager.RemoveAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME);
            await _userManager.SetAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME, newAccessToken);


            return new TokenVM
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        /// <inheritdoc />
        /// <exception cref="NullReferenceException">Thrown when the user to update does not exist.</exception>
        public async Task UpdateAsync(string id, EditUserDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NullReferenceException("No User to update");
            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);
        }


        /// <summary>
        /// Retrieves the claims for a given user.
        /// </summary>
        /// <param name="user">The user whose claims are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the claims.</returns>
        private async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
        {
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            return claims;
        }
    }
}
