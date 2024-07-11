using EventSeller.DataLayer.EntitiesDto.Statistics;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ISectorsStatisticsService
    {
        public Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync(long eventId, int maxCount = 0);
        public Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityInHallAsync(long placeHallId, int maxCount = 0);
        public Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0);
    }
}
