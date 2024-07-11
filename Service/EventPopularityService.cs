using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;


namespace EventSeller.Services.Service
{

    public class EventPopularityService : IEventPopularityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EventPopularityService> _logger;

        public EventPopularityService(IUnitOfWork unitOfWork, ILogger<EventPopularityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventPopularityStatistic>> GetEventsPopularityByPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            _logger.LogInformation($"Getting events popularity by period from {startDateTime} to {endDateTime}");
            var result = await _unitOfWork.AnalyticsRepository.GetEventsWithMaxPopularityAsync(
                obj => obj.StartEventDateTime >= startDateTime && obj.EndEventDateTime <= endDateTime,
                obj => obj.PopularityStatistic.Popularity
            );
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} events popularity statistics for the specified period.");
            return result;
        }

        /// <inheritdoc />
        public async Task<EventTypePopularityStatisticDTO> GetEventTypeStatistic(long eventTypeId)
        {
            _logger.LogInformation($"Getting event type statistic for event type ID: {eventTypeId}");
            var result = await _unitOfWork.AnalyticsRepository.GetEventTypeWithMaxPopularityAsync(eventType => eventType.Id == eventTypeId);
            if (result == null)
            {
                _logger.LogWarning($"No event type found with ID: {eventTypeId}");
            }
            else
            {
                _logger.LogInformation($"Retrieved popularity statistic for event type ID: {eventTypeId}");
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventPopularityStatistic>> GetMostPopularEvents(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most popular events");
            var result = await _unitOfWork.AnalyticsRepository.GetEventsWithMaxPopularityAsync(null, obj => obj.PopularityStatistic.Popularity, topCount);
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most popular events.");
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetMostPopularEventTypes(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most popular event types");
            var result = await _unitOfWork.AnalyticsRepository.GetEventTypesWithPopularityAsync(null, obj => obj.PopularityStatistic.Popularity, topCount);
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most popular event types.");
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventPopularityStatistic>> GetMostRealizableEvents(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most realizable events");
            var result = await _unitOfWork.AnalyticsRepository.GetEventsWithMaxPopularityAsync(null, obj => obj.PopularityStatistic.Realization, topCount);
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most realizable events.");
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetMostRealizableEventTypes(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most realizable event types");
            var result = await _unitOfWork.AnalyticsRepository.GetEventTypesWithPopularityAsync(null, obj => obj.PopularityStatistic.Realization, topCount);
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most realizable event types.");
            return result;
        }
    }
}
