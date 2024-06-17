using DataLayer.Model;
using DataLayer.Models.PlaceHall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with the <see cref="PlaceHall"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IPlaceHallService
    {
        /// <summary>
        /// Retrieves a place hall by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place hall.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the place hall.</returns>
        Task<PlaceHall> GetByID(long id);

        /// <summary>
        /// Retrieves a collection of all place halls.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of place halls.</returns>
        Task<IEnumerable<PlaceHall>> GetPlaceHalls();

        /// <summary>
        /// Creates a new place hall.
        /// </summary>
        /// <param name="model">The data transfer object containing place hall details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Create(AddPlaceHallDto model);

        /// <summary>
        /// Updates an existing place hall.
        /// </summary>
        /// <param name="id">The identifier of the place hall to update.</param>
        /// <param name="model">The data transfer object containing updated place hall details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(long id, EditPlaceHallDto model);

        /// <summary>
        /// Deletes a place hall by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place hall to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(long id);
    }
}
