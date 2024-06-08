using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public interface IUserRolesService
    {
        /// <summary>
        /// Sets the role of a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="role">The role to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetRole(string id, string role);

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="role">The role to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveRole(string id, string role);

        /// <summary>
        /// Gets all roles available in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of all roles.</returns>
        Task<IEnumerable<string>> GetAllRoles();

    }
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
        public async Task RemoveRole(string id, string role)
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
        public async Task SetRole(string id, string role)
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
        public async Task<IEnumerable<string>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }
    }
}
