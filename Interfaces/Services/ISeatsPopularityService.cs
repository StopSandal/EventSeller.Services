using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ISeatsPopularityService
    {
        public Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityForEventAsync(long eventId, int maxCount = 0);
        public Task<IEnumerable<SeatPopularityDTO>> GetSeatsPopularityInHallAsync(long placeHallId, int maxCount = 0);
        public Task<IEnumerable<EventSeatPopularityDTO>> GetSeatsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0);
    }
}
