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
        Task<PlaceHall> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves a collection of all place halls.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of place halls.</returns>
        Task<IEnumerable<PlaceHall>> GetPlaceHallsAsync();

        /// <summary>
        /// Creates a new place hall.
        /// </summary>
        /// <param name="model">The data transfer object containing place hall details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(AddPlaceHallDto model);

        /// <summary>
        /// Updates an existing place hall.
        /// </summary>
        /// <param name="id">The identifier of the place hall to update.</param>
        /// <param name="model">The data transfer object containing updated place hall details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditPlaceHallDto model);

        /// <summary>
        /// Deletes a place hall by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the place hall to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// Asynchronously determines whether an PlaceHall with the specified identifier exists in the data source.
        /// </summary>
        /// <param name="id">The identifier of the PlaceHall to search for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains 
        /// a boolean value that indicates whether an PlaceHall with the specified identifier 
        /// exists in the data source.
        /// </returns>
        public Task<bool> DoesExistsByIdAsync(long id);

        /// <summary>
        /// Retrieves all TicketSeats within the specified PlaceHall that fall within the given row bounds.
        /// </summary>
        /// <param name="placeHallId">The ID of the PlaceHall to retrieve TicketSeats from.</param>
        /// <param name="minRow">The minimum row number to include.</param>
        /// <param name="maxRow">The maximum row number to include.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TicketSeat entities.</returns>
        public Task<IEnumerable<TicketSeat>> GetAllSeatsInRangeByIdAsync(long placeHallId, int minRow, int maxRow);
    }
}
