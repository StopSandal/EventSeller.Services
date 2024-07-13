using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ISeatsPopularityService"/>.
    /// </summary>
    public class SeatsPopularityService : ISeatsPopularityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SeatsPopularityService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatsPopularityService"/> class with the specified unit of work and logger.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public SeatsPopularityService(IUnitOfWork unitOfWork, ILogger<SeatsPopularityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityForEventAsync(long eventId, int maxCount = 0)
        {
            _logger.LogInformation("Fetching seat popularity for event with ID: {EventId}", eventId);
            return await _unitOfWork.PopularityAnalyticsRepository.GetSeatPopularityForEventAsync(
                obj => obj.PopularityStatistic.Popularity,
                obj => obj.EventSession.EventID == eventId,
                null,
                maxCount);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityInHallAsync(long placeHallId, int maxCount = 0)
        {
            _logger.LogInformation("Fetching seat popularity for hall with ID: {PlaceHallId}", placeHallId);
            return await _unitOfWork.PopularityAnalyticsRepository.GetSeatPopularityAsync(
                obj => obj.PopularityStatistic.Popularity,
                obj => obj.HallSector.PlaceHallID == placeHallId,
                maxCount);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventSeatPopularityDTO>> GetSeatsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0)
        {
            if (!eventIds.Any())
            {
                _logger.LogError("No events provided");
                throw new ArgumentNullException($"No events provided");
            }
            _logger.LogInformation("Fetching seat popularity for event groups at hall with ID: {PlaceHallId}", placeHallId);
            return await _unitOfWork.PopularityAnalyticsRepository.GetSeatPopularityForEventsGroupsAsync(
                obj => obj.PopularityStatistic.Popularity,
                tickets => eventIds.Contains(tickets.EventSession.EventID),
                obj => obj.HallSector.PlaceHallID == placeHallId,
                maxCount);
        }
    }
}