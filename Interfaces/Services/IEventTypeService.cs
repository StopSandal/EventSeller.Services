using DataLayer.Model;
using DataLayer.Models.PlaceHall;
using EventSeller.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Represents all actions with the <see cref="EventType"/> class.
    /// </summary>
    /// <remarks>All actions include CRUD operations</remarks>
    public interface IEventTypeService
    {
        /// <summary>
        /// Retrieves a event type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event type.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the event type.</returns>
        Task<EventType> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves a collection of all event types.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of event types.</returns>
        Task<IEnumerable<EventType>> GetPlaceHallsAsync();

        /// <summary>
        /// Creates a new event type.
        /// </summary>
        /// <param name="model">The data transfer object containing event type details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(AddEventTypeDto model);

        /// <summary>
        /// Updates an existing event type.
        /// </summary>
        /// <param name="id">The identifier of the event type to update.</param>
        /// <param name="model">The data transfer object containing updated event type details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditEventTypeDto model);

        /// <summary>
        /// Deletes a event type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event type to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
    }
}
