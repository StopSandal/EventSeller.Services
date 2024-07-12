using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    public class SalesAnalyticsRepository : ISalesAnalyticsRepository
    {
        private readonly SellerContext _context;

        public SalesAnalyticsRepository(SellerContext context)
        {
            _context = context;
        }

        public async Task<SalesStatisticsDTO> GetTicketsSalesAsync(Expression<Func<Ticket, bool>> ticketFilter)
        {
            var tickets = _context.Set<Ticket>()
                            .Where(ticketFilter);

            var query = tickets
                            .Select(ticket => new SalesStatisticsDTO
                            {
                                TotalTickets = tickets.Select(x => x.ID).Count(),
                                TotalSold = tickets.Select(x => x.isSold).Count(x => x),
                                TotalIncome = tickets
                                    .Select(x => new { x.isSold, x.Price })
                                    .Where(x => x.isSold)
                                    .Sum(x => x.Price),
                                MaxPossibleIncome = tickets.Select(x => x.Price).Sum(),
                            });
            var result = await query.ToListAsync();
            if (!result.Any())
            {
                throw new Exception("No tickets found.");
            }

            return result.FirstOrDefault();
        }
    }
}
