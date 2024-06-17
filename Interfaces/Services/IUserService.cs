using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.DataLayer.EntitiesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
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
    }
}
