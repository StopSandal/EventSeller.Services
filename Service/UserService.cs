using AutoMapper;
using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.DataLayer.EntitiesViewModel;
using EventSeller.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="addUserDto">Data transfer object containing user information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateUser(AddUserDto addUserDto);

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user.</returns>
        Task<User> GetUserByUserName(string userName);

        /// <summary>
        /// Updates a user by ID. Doesn't set password
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="model">The data transfer object containing updated user information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(string id, EditUserDto model);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(string id);

        /// <summary>
        /// Authenticates a user and returns a JWT token and Refresh token.
        /// </summary>
        /// <param name="user">The login view model containing user credentials.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token view model.</returns>
        Task<TokenVM> Login(LoginUserVM user);

        /// <summary>
        /// Refreshes the JWT token.
        /// </summary>
        /// <param name="token">The token view model containing the access token and refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the new token view model.</returns>
        Task<TokenVM> RefreshToken(TokenVM token);

        /// <summary>
        /// Retrieves the roles for a specified user by their username.
        /// </summary>
        /// <param name="userName">The username of the user whose roles are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of roles for the specified user.</returns>
        Task<IEnumerable<string>> GetUserRoles(string userName);
    }

    
    /// <summary>
    /// Service class for user-related operations. Include Login, RefreshToken and CRUD operation. Also You can get UserRoles.
    /// </summary>
    public class UserService : IUserService
    {
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
        public async Task CreateUser(AddUserDto addUserDto)
        {
            if(await GetUserByUserName(addUserDto.UserName) != null)
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
            await _userManager.AddToRoleAsync(user, "Basic");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Can't assign basic user role {result.Errors}");
            }
        }
        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist or deletion fails.</exception>
        public async Task Delete(string id)
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
        public async Task<User> GetUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when the user does not exist or the password is incorrect.</exception>
        public async Task<TokenVM> Login(LoginUserVM user)
        {
            
            var existingUser = await GetUserByUserName(user.UserName) ?? await _userManager.FindByEmailAsync(user.Email);
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
            await _userManager.RemoveAuthenticationTokenAsync(existingUser, "Server", "jwt");
            await _userManager.SetAuthenticationTokenAsync(existingUser, "Server", "jwt", token);
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
        public async Task<TokenVM> RefreshToken(TokenVM token)
        {
            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var identity = await _JWTFactory.GetIdentityFromExpiredTokenAsync(accessToken);

            if (identity == null)
            {
                throw new InvalidOperationException("Invalid access token");
            }

            string username = identity.Name;

            var user = await GetUserByUserName(username);

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
            await _userManager.RemoveAuthenticationTokenAsync(user, "Server", "jwt");
            await _userManager.SetAuthenticationTokenAsync(user, "Server", "jwt", newAccessToken);


            return new TokenVM
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        /// <inheritdoc />
        /// <exception cref="NullReferenceException">Thrown when the user to update does not exist.</exception>
        public async Task Update(string id, EditUserDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NullReferenceException("No User to update");
            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);
        }

        ///<inheritdoc/>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist.</exception>
        public async Task<IEnumerable<string>> GetUserRoles(string userName)
        {
            // Assuming there is a way to get the current user, e.g., via a service or context.
            var user = await GetUserByUserName(userName);
            if (user == null)
            {
                throw new InvalidOperationException("Current user is not available.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles;
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
