using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the service for retrieving ticket sales statistics.
    /// </summary>
    public class TicketSalesStatisticService : ITicketSalesStatisticService
    {
        private readonly ILogger<TicketSalesStatisticService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSalesStatisticService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public TicketSalesStatisticService(IUnitOfWork unitOfWork, ILogger<TicketSalesStatisticService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId)
        {
            _logger.LogInformation("Fetching sales statistics for event with ID: {EventId}", eventId);
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => ticket.EventSession.EventID == eventId);
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAndSessionAsync(long eventId, long eventSessionId)
        {
            _logger.LogInformation("Fetching sales statistics for event with ID: {EventId} and session ID: {EventSessionId}", eventId, eventSessionId);
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(
                ticket => ticket.EventSession.EventID == eventId
                && ticket.EventSessionID == eventSessionId
                );
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventsAsync(IEnumerable<long> eventIds)
        {
            if (!eventIds.Any())
            {
                _logger.LogError("No events provided");
                throw new ArgumentNullException($"No events provided");
            }
            _logger.LogInformation("Fetching sales statistics for events.");
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => eventIds.Contains(ticket.EventSession.EventID));
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId)
        {
            _logger.LogInformation("Fetching sales statistics for event session with ID: {EventSessionId}", eventSessionId);
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => ticket.EventSessionID == eventSessionId);
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionsAsync(IEnumerable<long> eventSessionIds)
        {
            if (!eventSessionIds.Any())
            {
                _logger.LogError("No event sessions provided");
                throw new ArgumentNullException($"No events provided");
            }
            _logger.LogInformation("Fetching sales statistics for event sessions.");
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => eventSessionIds.Contains(ticket.EventSessionID));
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId)
        {
            _logger.LogInformation("Fetching sales statistics for event type with ID: {EventTypeId}", eventTypeId);
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(ticket => ticket.EventSession.Event.EventTypeID == eventTypeId);
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod)
        {
            _logger.LogInformation("Fetching sales statistics for period from {FirstPeriod} to {SecondPeriod}", firstPeriod, secondPeriod);
            return await _unitOfWork.SalesAnalyticsRepository.GetTicketsSalesAsync(
                ticket => ticket.EventSession.StartSessionDateTime > firstPeriod
                && ticket.EventSession.StartSessionDateTime <= secondPeriod);
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate)
        {
            _logger.LogInformation("Fetching sales statistics for week starting on {WeekStartDate}", weekStartDate);
            var weekEndDate = weekStartDate.Date.AddDays(7);
            return await GetSalesStatisticForPeriodAsync(weekStartDate, weekEndDate);
        }

        /// <inheritdoc/>
        public async Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate)
        {
            _logger.LogInformation("Fetching sales statistics for day: {DayDate}", dayDate);
            var dayStartTime = dayDate.Date;
            var dayEndTime = dayStartTime.AddDays(1);
            return await GetSalesStatisticForPeriodAsync(dayStartTime, dayEndTime);
        }
    }
}