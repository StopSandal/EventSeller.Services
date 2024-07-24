using EventSeller.DataLayer.Entities;
using EventSeller.Services.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service class for managing user roles.
    /// </summary>
    public class UserRolesService : IUserRolesService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserRolesService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRolesService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="logger">The logger.</param>
        public UserRolesService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserRolesService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the user ID or role is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist, does not have the specified role, or removal fails.</exception>
        public async Task RemoveRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(role))
                throw new ArgumentException($"'{nameof(role)}' cannot be null or empty.", nameof(role));

            _logger.LogInformation("Removing role '{Role}' from user '{UserId}'", role, userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User '{UserId}' not found.", userId);
                throw new InvalidOperationException("User doesn't exist.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(role))
            {
                _logger.LogWarning("User '{UserId}' does not have role '{Role}'.", userId, role);
                throw new InvalidOperationException("User doesn't have this role.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to remove role '{Role}' from user '{UserId}': {Errors}", role, userId, result.Errors.ToString());
                throw new InvalidOperationException($"Failed to remove role. {result.Errors.ToString()}");
            }

            _logger.LogInformation("Role '{Role}' removed from user '{UserId}' successfully.", role, userId);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the user ID or role is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist, already has the specified role, or assignment fails.</exception>
        public async Task SetRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(role))
                throw new ArgumentException($"'{nameof(role)}' cannot be null or empty.", nameof(role));

            _logger.LogInformation("Setting role '{Role}' for user '{UserId}'", role, userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User '{UserId}' not found.", userId);
                throw new InvalidOperationException("User doesn't exist.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(role))
            {
                _logger.LogWarning("User '{UserId}' already has role '{Role}'.", userId, role);
                throw new InvalidOperationException("User already has this role.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to set role '{Role}' for user '{UserId}': {Errors}", role, userId, result.Errors.ToString());
                throw new InvalidOperationException($"Failed to set role. {result.Errors.ToString()}");
            }

            _logger.LogInformation("Role '{Role}' set for user '{UserId}' successfully.", role, userId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            _logger.LogInformation("Fetching all roles.");
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown when the user name is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist.</exception>
        public async Task<IEnumerable<string>> GetUserRolesByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty.", nameof(userName));

            _logger.LogInformation("Fetching roles for user '{UserName}'", userName);

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                _logger.LogWarning("User '{UserName}' not found.", userName);
                throw new InvalidOperationException("User doesn't exist.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}