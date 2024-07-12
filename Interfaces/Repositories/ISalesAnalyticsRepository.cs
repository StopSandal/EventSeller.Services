using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    public interface ISalesAnalyticsRepository
    {
        public Task<SalesStatisticsDTO> GetTicketsSalesAsync(Expression<Func<Ticket, bool>> ticketFilter);
    }
}
