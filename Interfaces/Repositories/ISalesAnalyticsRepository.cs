using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving sales statistics.
    /// </summary>
    public interface ISalesAnalyticsRepository
    {
        /// <summary>
        /// Retrieves ticket sales statistics based on a specified filter.
        /// </summary>
        /// <param name="ticketFilter">The filter to apply to the tickets.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the sales statistics.</returns>
        Task<SalesStatisticsDTO> GetTicketsSalesAsync(Expression<Func<Ticket, bool>> ticketFilter);
    }
}
