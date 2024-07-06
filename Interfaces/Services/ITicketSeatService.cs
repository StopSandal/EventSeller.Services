using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.TicketSeat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with the <see cref="TicketSeat"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface ITicketSeatService
    {
        /// <summary>
        /// Retrieves a ticket seat by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket seat.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ticket seat.</returns>
        Task<TicketSeat> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves a collection of all ticket seats.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of ticket seats.</returns>
        Task<IEnumerable<TicketSeat>> GetTicketSeatsAsync();

        /// <summary>
        /// Creates a new ticket seat.
        /// </summary>
        /// <param name="model">The data transfer object containing ticket seat details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(AddTicketSeatDto model);

        /// <summary>
        /// Updates an existing ticket seat.
        /// </summary>
        /// <param name="id">The identifier of the ticket seat to update.</param>
        /// <param name="model">The data transfer object containing updated ticket seat details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditTicketSeatDto model);

        /// <summary>
        /// Deletes a ticket seat by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket seat to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
    }
}
