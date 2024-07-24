using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Implements the <see cref="IEventPopularityService"/> interface to provide methods for retrieving events popularity statistics.
    /// </summary>
    public class EventPopularityService : IEventPopularityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EventPopularityService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPopularityService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public EventPopularityService(IUnitOfWork unitOfWork, ILogger<EventPopularityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="startDateTime"/> or <paramref name="endDateTime"/> is null.</exception>
        public async Task<IEnumerable<EventPopularityStatistic>> GetEventsPopularityByPeriodAsync(DateTime startDateTime, DateTime endDateTime)
        {
            _logger.LogInformation($"Getting events popularity by period from {startDateTime} to {endDateTime}");
            var result = await _unitOfWork.PopularityAnalyticsRepository.GetEventsWithMaxPopularityAsync(
                obj => obj.StartEventDateTime >= startDateTime && obj.EndEventDateTime <= endDateTime,
                obj => obj.PopularityStatistic.Popularity
            );
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} events popularity statistics for the specified period.");
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventTypeId"/> is less than or equal to zero.</exception>
        public async Task<EventTypePopularityStatisticDTO> GetEventTypeStatisticAsync(long eventTypeId)
        {
            _logger.LogInformation($"Getting event type statistic for event type ID: {eventTypeId}");
            var result = await _unitOfWork.PopularityAnalyticsRepository.GetEventTypesWithPopularityAsync(
                eventType => eventType.Id == eventTypeId,
                x => x.PopularityStatistic.Popularity,
                1
            );
            if (result == null)
            {
                _logger.LogWarning($"No event type found with ID: {eventTypeId}");
            }
            else
            {
                _logger.LogInformation($"Retrieved popularity statistic for event type ID: {eventTypeId}");
            }
            return result.FirstOrDefault();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="topCount"/> is less than or equal to zero.</exception>
        public async Task<IEnumerable<EventPopularityStatistic>> GetMostPopularEventsAsync(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most popular events");
            var result = await _unitOfWork.PopularityAnalyticsRepository.GetEventsWithMaxPopularityAsync(
                null,
                obj => obj.PopularityStatistic.Popularity,
                topCount
            );
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most popular events.");
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="topCount"/> is less than or equal to zero.</exception>
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetMostPopularEventTypesAsync(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most popular event types");
            var result = await _unitOfWork.PopularityAnalyticsRepository.GetEventTypesWithPopularityAsync(
                null,
                obj => obj.PopularityStatistic.Popularity,
                topCount
            );
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most popular event types.");
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="topCount"/> is less than or equal to zero.</exception>
        public async Task<IEnumerable<EventPopularityStatistic>> GetMostRealizableEventsAsync(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most realizable events");
            var result = await _unitOfWork.PopularityAnalyticsRepository.GetEventsWithMaxPopularityAsync(
                null,
                obj => obj.PopularityStatistic.Realization,
                topCount
            );
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most realizable events.");
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="topCount"/> is less than or equal to zero.</exception>
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetMostRealizableEventTypesAsync(int topCount)
        {
            _logger.LogInformation($"Getting top {topCount} most realizable event types");
            var result = await _unitOfWork.PopularityAnalyticsRepository.GetEventTypesWithPopularityAsync(
                null,
                obj => obj.PopularityStatistic.Realization,
                topCount
            );
            _logger.LogInformation($"Retrieved {result?.Count() ?? 0} most realizable event types.");
            return result;
        }
    }
}
