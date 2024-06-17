using DataLayer.Model;
using DataLayer.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with the <see cref="Ticket"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface ITicketService
    {
        /// <summary>
        /// Retrieves a ticket by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ticket.</returns>
        Task<Ticket> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves a collection of all tickets.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of tickets.</returns>
        Task<IEnumerable<Ticket>> GetTicketsAsync();

        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="model">The data transfer object containing ticket details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(AddTicketDto model);

        /// <summary>
        /// Updates an existing ticket.
        /// </summary>
        /// <param name="id">The identifier of the ticket to update.</param>
        /// <param name="model">The data transfer object containing updated ticket details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditTicketDto model);

        /// <summary>
        /// Deletes a ticket by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the ticket to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
    }
}
