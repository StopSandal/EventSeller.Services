using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces
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
        public Task<IEnumerable<DaysStatistics>> GetDaysWithTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, Expression<Func<EventSession, bool>>? eventsFilter = null, int maxCount = 0);
        public Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<TicketSeat, bool>>? eventsFilter = null, int maxCount = 0);
        public Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityForEventAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<TicketSeat, bool>>? seatsFilter = null, int maxCount = 0);
        public Task<IEnumerable<EventSeatPopularityDTO>> GetSeatPopularityForEventsGroupsAsync<TField>(Func<SeatPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<TicketSeat, bool>>? seatsFilter = null, int maxCount = 0);
        public Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);
        public Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);
        public Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityForEventsGroupsAsync<TField>(Func<SectorPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);



    }
}
