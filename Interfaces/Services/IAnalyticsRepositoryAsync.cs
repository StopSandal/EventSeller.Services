using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Interface for the Analytics Repository, providing methods to retrieve event and event type popularity statistics.
    /// </summary>
    public interface IAnalyticsRepositoryAsync
    {
        /// <summary>
        /// Gets events with the highest popularity based on the specified filter and order criteria.
        /// </summary>
        /// <param name="eventsFilter">An optional filter expression for the events.</param>
        /// <param name="orderBy">An optional order expression for the popularity statistics.</param>
        /// <param name="maxCount">The maximum number of events to retrieve.</param>
        /// <returns>A collection of event popularity statistics.</returns>
        public Task<IEnumerable<EventPopularityStatistic>> GetEventsWithMaxPopularityAsync(Expression<Func<Event, bool>>? eventsFilter = null, Expression<Func<EventPopularityStatistic, decimal>>? orderBy = null, int maxCount = 0);

        /// <summary>
        /// Gets event types with popularity statistics based on the specified filter and order criteria.
        /// </summary>
        /// <param name="eventsFilter">An optional filter expression for the event types.</param>
        /// <param name="orderBy">An optional order expression for the popularity statistics.</param>
        /// <param name="maxCount">The maximum number of event types to retrieve.</param>
        /// <returns>A collection of event type popularity statistics.</returns>
        public Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0);

        /// <summary>
        /// Gets the event type with the highest popularity based on the specified filter criteria.
        /// </summary>
        /// <param name="eventTypeFilter">A filter expression for the event type.</param>
        /// <returns>The popularity statistics for the event type with the highest popularity.</returns>
        public Task<EventTypePopularityStatisticDTO> GetEventTypeWithMaxPopularityAsync(Expression<Func<EventType, bool>> eventTypeFilter);
    }
}
