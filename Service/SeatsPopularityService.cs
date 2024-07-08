using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;


namespace EventSeller.Services.Service
{
    public class SeatsPopularityService : ISeatsPopularityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SeatsPopularityService> _logger;

        public SeatsPopularityService(IUnitOfWork unitOfWork, ILogger<SeatsPopularityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityForEventAsync(long eventId, int maxCount = 0)
        {
            return await _unitOfWork.AnalyticsRepository.GetSeatPopularityForEventAsync(obj => obj.PopularityStatistic.Popularity, obj => obj.EventSession.EventID==eventId, null,  maxCount);
        }
        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityInHallAsync(long placeHallId, int maxCount = 0)
        {
            return await _unitOfWork.AnalyticsRepository.GetSeatPopularityAsync(obj => obj.PopularityStatistic.Popularity, obj => obj.HallSector.PlaceHallID==placeHallId, maxCount);
        }
        public async Task<IEnumerable<EventSeatPopularityDTO>> GetSeatsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0)
        {
            return await _unitOfWork.AnalyticsRepository.GetSeatPopularityForEventsGroupsAsync(obj => obj.PopularityStatistic.Popularity,tickets => eventIds.Contains(tickets.EventSession.EventID) , obj => obj.HallSector.PlaceHallID == placeHallId, maxCount);
        }
    }
}
