using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="IEventPopularityRepository"/> interface to provide methods for retrieving popularity data for <see cref="Event"/>.
    /// </summary>
    public class EventPopularityRepository : IEventPopularityRepository
    {
        private readonly SellerContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPopularityRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EventPopularityRepository(SellerContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no events are found with sold tickets.</exception>
        public async Task<IEnumerable<EventPopularityStatistic>> GetEventsWithMaxPopularityAsync(Expression<Func<Event, bool>>? eventsFilter = null, Expression<Func<EventPopularityStatistic, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<Event> events = _context.Set<Event>();

            if (eventsFilter != null)
            {
                events.Where(eventsFilter);
            }

            var query = events
                .Select(eventEntity => new
                {
                    EventId = eventEntity.ID,
                    Tickets = eventEntity.EventSessions.SelectMany(x => x.Tickets).AsQueryable().ToList()
                })
                .Where(e => e.Tickets.Any())
                .Select(events => new
                {
                    EventId = events.EventId,
                    TotalTickets = events.Tickets.Count(),
                    TotalSold = events.Tickets.Count(ticket => ticket.isSold),
                    PossibleIncome = events.Tickets.Sum(ticket => ticket.Price),
                    TotalIncome = events.Tickets.Where(ticket => ticket.isSold).Sum(ticket => ticket.Price)

                });


            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null || !eventsWithPopularity.Any())
            {
                throw new InvalidDataException("No seats found.");
            }

            if (orderBy == null)
            {
                orderBy = x => x.PopularityStatistic.Popularity;
            }

            var result = eventsWithPopularity
                .Select(e => new EventPopularityStatistic
                {
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.TotalSold / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.TotalSold,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = ((decimal)e.TotalSold / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    },
                    EventId = e.EventId
                })
                .OrderByDescending(orderBy.Compile());
            return result;
        }
    }
}
