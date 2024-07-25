using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ISectorsStatisticsService"/>.
    /// </summary>
    public class SectorsStatisticsService : ISectorsStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SectorsStatisticsService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorsStatisticsService"/> class with the specified unit of work and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public SectorsStatisticsService(IUnitOfWork unitOfWork, ILogger<SectorsStatisticsService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync(long eventId, int maxCount = 0)
        {
            _logger.LogInformation("Fetching sector popularity for event with ID: {EventId}", eventId);
            return await _unitOfWork.SectorPopularityRepository.GetSectorsPopularityForEventAsync(
                obj => obj.PopularityStatistic.Popularity,
                obj => obj.EventSession.EventID == eventId,
                null,
                maxCount);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityInHallAsync(long placeHallId, int maxCount = 0)
        {
            _logger.LogInformation("Fetching sector popularity for hall with ID: {PlaceHallId}", placeHallId);
            return await _unitOfWork.SectorPopularityRepository.GetSectorsPopularityAsync(
                obj => obj.PopularityStatistic.Popularity,
                obj => obj.PlaceHallID == placeHallId,
                maxCount);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0)
        {
            if (!eventIds.Any())
            {
                _logger.LogError("No events provided");
                throw new ArgumentNullException($"No events provided");
            }
            _logger.LogInformation("Fetching sector popularity for event groups at hall with ID: {PlaceHallId}", placeHallId);
            return await _unitOfWork.SectorPopularityRepository.GetSectorsPopularityForEventsGroupsAsync(
                obj => obj.PopularityStatistic.Popularity,
                tickets => eventIds.Contains(tickets.EventSession.EventID),
                obj => obj.PlaceHallID == placeHallId,
                maxCount);
        }
    }
}