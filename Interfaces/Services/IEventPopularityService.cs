using EventSeller.DataLayer.EntitiesDto.Statistics;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Interface for the Event Popularity Service, which provides methods to retrieve event popularity statistics.
    /// </summary>
    public interface IEventPopularityService
    {
        /// <summary>
        /// Gets the popularity statistics for events within a specified time period.
        /// </summary>
        /// <param name="startDateTime">The start date and time of the period.</param>
        /// <param name="endDateTime">The end date and time of the period.</param>
        /// <returns>A collection of event popularity statistics.</returns>
        public Task<IEnumerable<EventPopularityStatistic>> GetEventsPopularityByPeriodAsync(DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// Gets the popularity statistics for a specific event type.
        /// </summary>
        /// <param name="eventTypeId">The ID of the event type.</param>
        /// <returns>The popularity statistics for the specified event type.</returns>
        public Task<EventTypePopularityStatisticDTO> GetEventTypeStatisticAsync(long eventTypeId);

        /// <summary>
        /// Gets the most popular events based on their popularity statistics.
        /// </summary>
        /// <param name="topCount">The number of top events to retrieve.</param>
        /// <returns>A collection of the most popular events.</returns>
        public Task<IEnumerable<EventPopularityStatistic>> GetMostPopularEventsAsync(int topCount);

        /// <summary>
        /// Gets the most popular event types based on their popularity statistics.
        /// </summary>
        /// <param name="topCount">The number of top event types to retrieve.</param>
        /// <returns>A collection of the most popular event types.</returns>
        public Task<IEnumerable<EventTypePopularityStatisticDTO>> GetMostPopularEventTypesAsync(int topCount);

        /// <summary>
        /// Gets the most realizable events based on their realization statistics.
        /// </summary>
        /// <param name="topCount">The number of top events to retrieve.</param>
        /// <returns>A collection of the most realizable events.</returns>
        public Task<IEnumerable<EventPopularityStatistic>> GetMostRealizableEventsAsync(int topCount);

        /// <summary>
        /// Gets the most realizable event types based on their realization statistics.
        /// </summary>
        /// <param name="topCount">The number of top event types to retrieve.</param>
        /// <returns>A collection of the most realizable event types.</returns>
        public Task<IEnumerable<EventTypePopularityStatisticDTO>> GetMostRealizableEventTypesAsync(int topCount);
    }
}
