using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    public class TicketSalesStatisticService : ITicketSalesStatisticService
    {
        private readonly ILogger<TicketSalesStatisticService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TicketSalesStatisticService(IUnitOfWork unitOfWork, ILogger<TicketSalesStatisticService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => ticket.EventSession.EventID == eventId);
        }
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAndSessionAsync(long eventId, long eventSessionId)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(
                ticket => ticket.EventSession.EventID == eventId
                && ticket.EventSessionID == eventSessionId
                );
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventsAsync(IEnumerable<long> eventIds)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => eventIds.Contains(ticket.EventSession.EventID));
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => ticket.EventSessionID == eventSessionId);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionsAsync(IEnumerable<long> eventSessionIds)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => eventSessionIds.Contains(ticket.EventSessionID));
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => ticket.EventSession.Event.EventTypeID == eventTypeId);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod)
        {
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(
                ticket => ticket.EventSession.StartSessionDateTime > firstPeriod
                && ticket.EventSession.StartSessionDateTime <= secondPeriod);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate)
        {
            var weekEndDate = weekStartDate.Date.AddDays(7);
            return await GetSalesStatisticForPeriodAsync(weekStartDate, weekEndDate);
        }
        public async Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate)
        {
            var dayStartTime = dayDate.Date;
            var dayEndTime = dayStartTime.AddDays(1);
            return await GetSalesStatisticForPeriodAsync(dayStartTime, dayEndTime);
        }
    }
}
