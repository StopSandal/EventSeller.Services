﻿namespace EventSeller.Services.Interfaces.Services
{
    public interface IUserRolesService
    {
        /// <summary>
        /// Sets the role of a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="role">The role to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetRoleAsync(string id, string role);

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="role">The role to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveRoleAsync(string id, string role);

        /// <summary>
        /// Gets all roles available in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of all roles.</returns>
        Task<IEnumerable<string>> GetAllRolesAsync();

        /// <summary>
        /// Retrieves the roles for a specified user by their username.
        /// </summary>
        /// <param name="userName">The username of the user whose roles are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of roles for the specified user.</returns>
        Task<IEnumerable<string>> GetUserRolesByUserNameAsync(string userName);

    }
}
