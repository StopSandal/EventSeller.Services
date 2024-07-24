using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Service interface for retrieving day traffic statistics.
    /// </summary>
    public interface IDayTrafficStatisticService
    {
        /// <summary>
        /// Retrieves traffic statistics for days, optionally ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of traffic statistics for days.</returns>
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, int maxCount);

        /// <summary>
        /// Retrieves traffic statistics for days within a specified period, optionally ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="startPeriod">The start date of the period.</param>
        /// <param name="endPeriod">The end date of the period.</param>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of traffic statistics for days within the specified period.</returns>
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPeriodAsync<TField>(DateTime startPeriod, DateTime endPeriod, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0);

        /// <summary>
        /// Retrieves traffic statistics for days at a specific hall, optionally ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="placeHallId">The ID of the place hall.</param>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of traffic statistics for days at the specified hall.</returns>
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtHallAsync<TField>(long placeHallId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0);

        /// <summary>
        /// Retrieves traffic statistics for days at a specific place address, optionally ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="placeAddressId">The ID of the place address.</param>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of traffic statistics for days at the specified place address.</returns>
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPlaceAsync<TField>(long placeAddressId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0);
    }
}
