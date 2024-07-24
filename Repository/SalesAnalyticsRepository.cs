using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="ISalesAnalyticsRepository"/> interface to provide methods for retrieving sales statistics related to tickets.
    /// </summary>
    public class SalesAnalyticsRepository : ISalesAnalyticsRepository
    {
        private readonly SellerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesAnalyticsRepository"/> class with the specified context and logger.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SalesAnalyticsRepository(SellerContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown when no tickets matching the specified filter are found.</exception>
        public async Task<SalesStatisticsDTO> GetTicketsSalesAsync(Expression<Func<Ticket, bool>> ticketFilter)
        {
            var tickets = _context.Set<Ticket>()
                            .Where(ticketFilter);

            var query = tickets
                            .Select(ticket => new SalesStatisticsDTO
                            {
                                TotalTickets = tickets.Count(),
                                TotalSold = tickets.Count(x => x.isSold),
                                TotalIncome = tickets
                                    .Where(x => x.isSold)
                                    .Sum(x => x.Price),
                                MaxPossibleIncome = tickets.Sum(x => x.Price),
                            });

            var result = await query.FirstOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("No tickets found.");
            }

            return result;
        }
    }
}
