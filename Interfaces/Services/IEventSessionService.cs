using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.EventSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface IEventSessionService
    {
        /// <summary>
        /// Retrieves an session by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the session.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the session.</returns>
        Task<EventSession> GetByIDAsync(long id);

        /// <summary>
        /// Retrieves a collection of all sessions.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of sessions.</returns>
        Task<IEnumerable<EventSession>> GetEventSessionsAsync();
        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="model">The data transfer object containing session details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the session.</returns>
        Task<EventSession> CreateAsync(AddEventSessionDTO model);
        /// <summary>
        /// Updates an existing session.
        /// </summary>
        /// <param name="id">The identifier of the session to update.</param>
        /// <param name="model">The data transfer object containing updated session details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(long id, EditEventSessionDTO model);
        /// <summary>
        /// Deletes an session by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the session to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// Retrieves a collection of values for a specific field from event session that match the filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field to retrieve.</typeparam>
        /// <param name="filter">An expression to filter the event sessions.</param>
        /// <param name="selector">An expression to select the field to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of field values.</returns>
        public Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<EventSession, bool>> filter, Expression<Func<EventSession, TField>> selector);
    }
}
