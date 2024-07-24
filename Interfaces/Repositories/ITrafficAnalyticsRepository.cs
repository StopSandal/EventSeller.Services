using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving traffic analytics data.
    /// </summary>
    public interface ITrafficAnalyticsRepository
    {
        /// <summary>
        /// Retrieves traffic statistics for specified days, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="eventsFilter">An optional filter to apply to event sessions.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of traffic statistics for the specified days.</returns>
        Task<IEnumerable<DaysStatistics>> GetDaysWithTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, Expression<Func<EventSession, bool>>? eventsFilter = null, int maxCount = 0);
    }
}
