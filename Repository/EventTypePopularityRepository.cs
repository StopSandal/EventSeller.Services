using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="IEventTypePopularityRepository"/> interface to provide methods for retrieving popularity data for <see cref="EventType"/>.
    /// </summary>
    public class EventTypePopularityRepository : IEventTypePopularityRepository
    {
        private readonly SellerContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypePopularityRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EventTypePopularityRepository(SellerContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no event types are found with sold tickets.</exception>
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<EventType> eventTypes = _context.Set<EventType>();

            if (eventsFilter != null)
            {
                eventTypes = eventTypes.Where(eventsFilter);
            }

            var query = eventTypes
                .Select(eventTypeEntity => new
                {
                    EventType = eventTypeEntity,
                    Tickets = eventTypeEntity.Event.SelectMany(e => e.EventSessions).SelectMany(es => es.Tickets).ToList()
                })
                .Where(e => e.Tickets.Any())
                .Select(e => new
                {
                    EventTypeId = e.EventType.Id,
                    TotalTickets = e.Tickets.Count,
                    SoldCount = e.Tickets.Count(ticket => ticket.isSold),
                    PossibleIncome = e.Tickets.Sum(ticket => ticket.Price),
                    TotalIncome = e.Tickets.Where(ticket => ticket.isSold).Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalTickets > 0);

            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null || !eventsWithPopularity.Any())
            {
                throw new InvalidDataException("No event types found with sold tickets.");
            }

            if (orderBy == null)
            {
                orderBy = x => x.PopularityStatistic.Popularity;
            }

            var resultEntities = eventsWithPopularity
                .Select(e => new EventTypePopularityStatisticDTO
                {
                    EventTypeId = e.EventTypeId,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.SoldCount,
                        Monetization = e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome,
                        Popularity = (decimal)e.SoldCount / e.TotalTickets * (e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome)
                    }
                })
                .OrderByDescending(orderBy.Compile())
                .ToList();

            return resultEntities;
        }
    }
}
