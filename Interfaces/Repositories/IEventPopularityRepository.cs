using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving popularity data for event.
    /// </summary>
    public interface IEventPopularityRepository
    {
        /// <summary>
        /// Retrieves events with maximum popularity, optionally filtered and ordered.
        /// </summary>
        /// <param name="eventsFilter">An optional filter to apply to events.</param>
        /// <param name="orderBy">An optional expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of event popularity statistics.</returns>
        Task<IEnumerable<EventPopularityStatistic>> GetEventsWithMaxPopularityAsync(Expression<Func<Event, bool>>? eventsFilter = null, Expression<Func<EventPopularityStatistic, decimal>>? orderBy = null, int maxCount = 0);
    }
}
