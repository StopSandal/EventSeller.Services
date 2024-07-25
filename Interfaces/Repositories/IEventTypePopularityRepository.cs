using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving popularity data for event type.
    /// </summary>
    public interface IEventTypePopularityRepository
    {
        /// <summary>
        /// Retrieves event types with popularity statistics, optionally filtered and ordered.
        /// </summary>
        /// <param name="eventsFilter">An optional filter to apply to event types.</param>
        /// <param name="orderBy">An optional expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of event type popularity statistics.</returns>
        Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0);
    }
}
