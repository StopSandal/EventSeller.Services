using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    public class TrafficAnalyticsRepository : ITrafficAnalyticsRepository
    {
        private readonly SellerContext _context;

        public TrafficAnalyticsRepository(SellerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DaysStatistics>> GetDaysWithTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, Expression<Func<EventSession, bool>>? eventsFilter = null, int maxCount = 0)
        {
            IQueryable<EventSession> eventSessions = _context.Set<EventSession>();

            if (eventsFilter != null)
            {
                eventSessions = eventSessions.Where(eventsFilter);
            }

            var query = eventSessions
                .SelectMany(item => item.Tickets.Select(ticket => ticket.isSold).Where(x => x), (item, ticket) => new { item, ticket })
                .GroupBy(x => x.item.StartSessionDateTime.Date)
                .Select(g => new DaysStatistics
                {
                    Date = g.Key,
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    TotalTraffic = g.Count()
                });

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var eventsWithPopularity = await query.OrderByDescending(orderBy).ToListAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No days found.");
            }

            return eventsWithPopularity;
        }
    }
}
