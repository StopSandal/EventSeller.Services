using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="ITrafficAnalyticsRepository"/> interface to provide methods for retrieving traffic analytics data.
    /// </summary>
    public class TrafficAnalyticsRepository : ITrafficAnalyticsRepository
    {
        private readonly SellerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrafficAnalyticsRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TrafficAnalyticsRepository(SellerContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no days matching the specified criteria are found.</exception>
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

            var daysWithTraffic = await query.OrderByDescending(orderBy).ToListAsync();

            if (daysWithTraffic == null || !daysWithTraffic.Any())
            {
                throw new InvalidDataException("No days found.");
            }

            return daysWithTraffic;
        }
    }
}
