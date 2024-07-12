using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving popularity analytics data.
    /// </summary>
    public interface IPopularityAnalyticsRepository
    {
        /// <summary>
        /// Retrieves events with maximum popularity, optionally filtered and ordered.
        /// </summary>
        /// <param name="eventsFilter">An optional filter to apply to events.</param>
        /// <param name="orderBy">An optional expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of event popularity statistics.</returns>
        Task<IEnumerable<EventPopularityStatistic>> GetEventsWithMaxPopularityAsync(Expression<Func<Event, bool>>? eventsFilter = null, Expression<Func<EventPopularityStatistic, decimal>>? orderBy = null, int maxCount = 0);

        /// <summary>
        /// Retrieves event types with popularity statistics, optionally filtered and ordered.
        /// </summary>
        /// <param name="eventsFilter">An optional filter to apply to event types.</param>
        /// <param name="orderBy">An optional expression used to order the results.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of event type popularity statistics.</returns>
        Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0);

        /// <summary>
        /// Retrieves seat popularity statistics, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="ticketSeatsFilter">An optional filter to apply to ticket seats.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of seat popularity statistics.</returns>
        Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<TicketSeat, bool>>? ticketSeatsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves seat popularity statistics for a specific event, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="ticketsFilter">The filter to apply to tickets related to the event.</param>
        /// <param name="seatsFilter">An optional filter to apply to ticket seats.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of seat popularity statistics.</returns>
        Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityForEventAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<TicketSeat, bool>>? seatsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves seat popularity statistics for groups of events, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The function used to order the results.</param>
        /// <param name="ticketsFilter">The filter to apply to tickets related to the events.</param>
        /// <param name="seatsFilter">An optional filter to apply to ticket seats.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of seat popularity statistics grouped by events.</returns>
        Task<IEnumerable<EventSeatPopularityDTO>> GetSeatPopularityForEventsGroupsAsync<TField>(Func<SeatPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<TicketSeat, bool>>? seatsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves hall sector popularity statistics, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="sectorsFilter">An optional filter to apply to hall sectors.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sector popularity statistics.</returns>
        Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves hall sector popularity statistics for a specific event, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="ticketsFilter">The filter to apply to tickets related to the event.</param>
        /// <param name="sectorsFilter">An optional filter to apply to hall sectors.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sector popularity statistics.</returns>
        Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves hall sector popularity statistics for groups of events, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The function used to order the results.</param>
        /// <param name="ticketsFilter">The filter to apply to tickets related to the events.</param>
        /// <param name="sectorsFilter">An optional filter to apply to hall sectors.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sector popularity statistics grouped by events.</returns>
        Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityForEventsGroupsAsync<TField>(Func<SectorPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);
    }
}
