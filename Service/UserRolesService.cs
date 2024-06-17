using EventSeller.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventSeller.Services.Interfaces.Services;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service class for managing user roles.
    /// </summary>
    public class UserRolesService : IUserRolesService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when the user does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not have the specified role or removal fails.</exception>
        public async Task RemoveRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new InvalidDataException("This user doesn't exists");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(role))
            {
                throw new InvalidOperationException("User doesn't have this role");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.ToString());
            }
        }
        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when the user does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user already has the specified role or assignment fails.</exception>
        public async Task SetRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new InvalidDataException("This user doesn't exists");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(role))
            {
                throw new InvalidOperationException("User already have this role");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.ToString());
            }
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }
        ///<inheritdoc/>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist.</exception>
        public async Task<IEnumerable<string>> GetUserRolesByUserNameAsync(string userName)
        {
            // Assuming there is a way to get the current user, e.g., via a service or context.
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new InvalidOperationException("Current user is not available.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
