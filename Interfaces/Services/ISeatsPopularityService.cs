using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Service interface for retrieving seats popularity statistics.
    /// </summary>
    public interface ISeatsPopularityService
    {
        /// <summary>
        /// Retrieves seat popularity statistics for a specific event, optionally limited by count.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of seat popularity statistics for the specified event.</returns>
        Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityForEventAsync(long eventId, int maxCount = 0);

        /// <summary>
        /// Retrieves seat popularity statistics for a specific place hall, optionally limited by count.
        /// </summary>
        /// <param name="placeHallId">The ID of the place hall.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of seat popularity statistics for the specified place hall.</returns>
        Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityInHallAsync(long placeHallId, int maxCount = 0);

        /// <summary>
        /// Retrieves seat popularity statistics grouped by event groups at a specific place hall, optionally limited by count.
        /// </summary>
        /// <param name="placeHallId">The ID of the place hall.</param>
        /// <param name="eventIds">The IDs of the events to group by.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of seat popularity statistics grouped by event groups at the specified place hall.</returns>
        Task<IEnumerable<EventSeatPopularityDTO>> GetSeatsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0);
    }
}
