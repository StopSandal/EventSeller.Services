using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ITicketSalesStatisticService
    {
        Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(IEnumerable<long> eventIds);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId);
        Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod);
        Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate);
        Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId);
    }
}
