using EventSeller.DataLayer.EntitiesDto.Statistics;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ITicketSalesStatisticService
    {
        Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventAndSessionAsync(long eventId, long eventSessionId);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventsAsync(IEnumerable<long> eventIds);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionsAsync(IEnumerable<long> eventSessionIds);
        Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod);
        Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate);
        Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate);
        Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId);
    }
}
