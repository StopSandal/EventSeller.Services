using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving popularity data for seat.
    /// </summary>
    public interface ISeatPopularityRepository
    {
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

    }
}
