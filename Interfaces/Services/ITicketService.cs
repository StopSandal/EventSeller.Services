using DataLayer.Model;
using DataLayer.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        /// <summary>
        /// Retrieves a ticket with it's parent properties by its identifier.
        /// </summary>
        /// <param name="ticketId">The identifier of the ticket.</param>
        /// <param name="includes">A comma-separated list of related entities to include in the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ticket.</returns>
        public Task<Ticket> GetTicketWithIncudesByIdAsync(long ticketId, string includes);

        /// <summary>
        /// Add a bunch of tickets.
        /// </summary>
        /// <param name="ticketList">Collection<see cref="IEnumerable{T}"/> of the tickets <see cref="Ticket"/> to insert .</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddTicketListAsync(IEnumerable<Ticket> ticketList);
        /// <summary>
        /// Asynchronously counts the number of tickets that match the specified filter.
        /// </summary>
        /// <param name="includeProperties">A list of related entities to include in the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the count of matching tickets.</returns>
        public Task<int> GetTicketCountAsync(Expression<Func<Ticket, bool>> filter = null, IEnumerable<string> includeProperties = null);
        /// <summary>
        /// Retrieves a collection of values for a specific field from ticket that match the filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field to retrieve.</typeparam>
        /// <param name="filter">An expression to filter the tickets.</param>
        /// <param name="selector">An expression to select the field to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of field values.</returns>
        public Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<Ticket, bool>> filter, Expression<Func<Ticket, TField>> selector);
    }
}
