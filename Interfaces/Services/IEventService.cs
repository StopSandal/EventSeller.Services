using DataLayer.Model;
using DataLayer.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with <see cref="Event"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IEventService
    {
        /// <summary>
        /// Retrieves an event by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the event.</returns>
        Task<Event> GetByIDAsync(long id);
        /// <summary>
        /// Retrieves a collection of all events.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of events.</returns>
        Task<IEnumerable<Event>> GetEventsAsync();
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="model">The data transfer object containing event details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(AddEventDto model);
        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="id">The identifier of the event to update.</param>
        /// <param name="model">The data transfer object containing updated event details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditEventDto model);
        /// <summary>
        /// Deletes an event by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// Asynchronously determines whether an event with the specified identifier exists in the data source.
        /// </summary>
        /// <param name="id">The identifier of the event to search for.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains 
        /// a boolean value that indicates whether an event with the specified identifier 
        /// exists in the data source.
        /// </returns>
        public Task<bool> DoesExistsByIdAsync(long id);
    }
}
