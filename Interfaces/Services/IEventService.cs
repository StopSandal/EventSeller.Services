using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Event;
using System.Linq.Expressions;

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
        /// <param name="includeProperties">A comma-separated list of related entities to include in the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the event.</returns>
        Task<Event> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves an event by its identifier with properties.
        /// </summary>
        /// <param name="id">The identifier of the event.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include in the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the event.</returns>
        Task<Event> GetWithIncludesByIDAsync(long id, string includeProperties = null);

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
        /// <summary>
        /// Retrieves a collection of values for a specific field from event that match the filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field to retrieve.</typeparam>
        /// <param name="filter">An expression to filter the events.</param>
        /// <param name="selector">An expression to select the field to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of field values.</returns>
        public Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<Event, bool>> filter, Expression<Func<Event, TField>> selector);
    }
}
