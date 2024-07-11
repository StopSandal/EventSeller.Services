using EventSeller.DataLayer.EntitiesDto.Statistics;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ISeatsPopularityService
    {
        public Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityForEventAsync(long eventId, int maxCount = 0);
        public Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityInHallAsync(long placeHallId, int maxCount = 0);
        public Task<IEnumerable<EventSeatPopularityDTO>> GetSeatsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0);
    }
}
