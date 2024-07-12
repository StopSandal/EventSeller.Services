﻿using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EventSeller.Services.Service
{
    public class SectorsStatisticsService : ISectorsStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SectorsStatisticsService> _logger;

        public SectorsStatisticsService(IUnitOfWork unitOfWork, ILogger<SectorsStatisticsService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync(long eventId, int maxCount = 0)
        {
            return await _unitOfWork.PopularityAnalyticsRepository.GetSectorsPopularityForEventAsync(obj => obj.PopularityStatistic.Popularity, obj => obj.EventSession.EventID == eventId, null, maxCount);
        }
        public async Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityInHallAsync(long placeHallId, int maxCount = 0)
        {
            return await _unitOfWork.PopularityAnalyticsRepository.GetSectorsPopularityAsync(obj => obj.PopularityStatistic.Popularity, obj => obj.PlaceHallID == placeHallId, maxCount);
        }
        public async Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityByEventGroupsAtHallAsync(long placeHallId, IEnumerable<long> eventIds, int maxCount = 0)
        {
            return await _unitOfWork.PopularityAnalyticsRepository.GetSectorsPopularityForEventsGroupsAsync(obj => obj.PopularityStatistic.Popularity, tickets => eventIds.Contains(tickets.EventSession.EventID), obj => obj.PlaceHallID == placeHallId, maxCount);
        }
    }
}
