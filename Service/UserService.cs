using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.Services.Helpers.Constants;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service class for user-related operations. Include Login, RefreshToken and CRUD operation. Also You can get UserRoles.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IJWTFactory _JWTFactory;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign-in manager.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="JWTFactory">The JWT factory.</param>
        /// <param name="logger">The logger.</param>
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IJWTFactory jWTFactory, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _JWTFactory = jWTFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the username or email is already in use or user creation fails.</exception>
        public async Task CreateUserAsync(AddUserDto addUserDto)
        {
            _logger.LogInformation("Creating a new user with username '{UserName}'", addUserDto.UserName);

            if (await GetUserByUserNameAsync(addUserDto.UserName) != null)
            {
                _logger.LogWarning("Username '{UserName}' is already taken.", addUserDto.UserName);
                throw new InvalidOperationException("This UserName is already taken.");
            }

            if (await _userManager.FindByEmailAsync(addUserDto.Email) != null)
            {
                _logger.LogWarning("Email '{Email}' is already in use.", addUserDto.Email);
                throw new InvalidOperationException("This Email is already in use.");
            }

            var user = new User
            {
                UserName = addUserDto.UserName,
                Email = addUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, addUserDto.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create user: {Errors}", result.Errors.ToString());
                throw new InvalidOperationException($"Failed to create user. {result.Errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, UsersConstants.UserBaseRole);
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to assign basic user role: {Errors}", roleResult.Errors.ToString());
                throw new InvalidOperationException($"Failed to assign basic user role. {result.Errors}");
            }

            _logger.LogInformation("User '{UserName}' created successfully.", user.UserName);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the user ID is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist or deletion fails.</exception>
        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));

            _logger.LogInformation("Deleting user with ID '{UserId}'", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID '{UserId}' not found.", id);
                throw new InvalidOperationException("User doesn't exist.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete user '{UserId}': {Errors}", id, result.Errors.ToString());
                throw new InvalidOperationException($"Failed to delete user. {result.Errors}");
            }

            _logger.LogInformation("User '{UserId}' deleted successfully.", id);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the username is null or empty.</exception>
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty.", nameof(userName));

            _logger.LogInformation("Fetching user with username '{UserName}'", userName);

            return await _userManager.FindByNameAsync(userName);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the user object is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs during login or token generation.</exception>
        public async Task<TokenDTO> LoginAsync(LoginUserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _logger.LogInformation("User '{UserName}' attempting to log in.", user.UserName);

            var existingUser = await _userManager.Users
                                        .FirstOrDefaultAsync(u => u.UserName == user.UserName || u.Email == user.Email);

            if (existingUser == null)
            {
                _logger.LogWarning("Login failed for '{UserName}': User not found.", user.UserName);
                throw new InvalidOperationException("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(existingUser, user.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed for '{UserName}': Invalid password.", user.UserName);
                throw new InvalidOperationException("Invalid password.");
            }

            try
            {
                // get claims and JWT token
                var claims = await GetUserClaimsAsync(existingUser);
                var token = _JWTFactory.GenerateToken(claims);

                //set JWT token
                await _userManager.RemoveAuthenticationTokenAsync(existingUser, UsersConstants.LoginProvider, UsersConstants.TokenName);
                await _userManager.SetAuthenticationTokenAsync(existingUser, UsersConstants.LoginProvider, UsersConstants.TokenName, token);

                //set Refresh token for user
                var refreshToken = _JWTFactory.GenerateRefreshToken();
                existingUser.RefreshToken = refreshToken;
                existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_JWTFactory.GetRefreshTokenValidityInDays());
                await _userManager.UpdateAsync(existingUser);

                _logger.LogInformation("User '{UserName}' logged in successfully.", user.UserName);

                return new TokenDTO()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user '{UserName}'.", user.UserName);
                throw new InvalidOperationException("An error occurred while logging in.", ex);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the token object is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the access token or refresh token is invalid.</exception>
        public async Task<TokenDTO> RefreshTokenAsync(TokenDTO token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            _logger.LogInformation("Refreshing token...");

            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var identity = await _JWTFactory.GetIdentityFromExpiredTokenAsync(accessToken);

            if (identity == null)
            {
                _logger.LogWarning("Invalid access token provided for token refresh.");
                throw new InvalidOperationException("Invalid access token");
            }

            string username = identity.Name;

            var user = await GetUserByUserNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid access token or refresh token provided for token refresh.");
                throw new InvalidOperationException("Invalid access token or refresh token");
            }

            try
            {
                // set new refresh token
                var newRefreshToken = _JWTFactory.GenerateRefreshToken();
                user.RefreshToken = newRefreshToken;
                await _userManager.UpdateAsync(user);

                // reset JWT token
                var newAccessToken = _JWTFactory.GenerateToken(await GetUserClaimsAsync(user));
                await _userManager.RemoveAuthenticationTokenAsync(user, UsersConstants.LoginProvider, UsersConstants.TokenName);
                await _userManager.SetAuthenticationTokenAsync(user, UsersConstants.LoginProvider, UsersConstants.TokenName, newAccessToken);

                _logger.LogInformation("Token refreshed successfully for user '{UserName}'.", user.UserName);

                return new TokenDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing the token for user '{UserName}'.", user?.UserName);
                throw new InvalidOperationException("An error occurred while refreshing the token.", ex);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the user ID is null or empty.</exception>
        /// <exception cref="NullReferenceException">Thrown when the user to update does not exist.</exception>
        public async Task UpdateAsync(string id, EditUserDto model)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));

            _logger.LogInformation("Updating user with ID '{UserId}'", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID '{UserId}' not found.", id);
                throw new NullReferenceException("No User to update");
            }

            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User '{UserId}' updated successfully.", id);
        }

        /// <summary>
        /// Retrieves the claims for a given user.
        /// </summary>
        /// <param name="user">The user whose claims are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the claims.</returns>
        /// <exception cref="ArgumentException">Thrown when the user object is null.</exception>
        private async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _logger.LogInformation("Getting user claims for '{UserName}'", user.UserName);

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

            _logger.LogInformation("Retrieved {ClaimCount} claims for user '{UserName}'.", claims.Count, user.UserName);

            return claims;
        }
    }
}