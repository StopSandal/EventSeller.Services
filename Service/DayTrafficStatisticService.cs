using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EventSeller.Services.Service
{
    public class DayTrafficStatisticService : IDayTrafficStatisticService
    {
        private readonly ILogger<DayTrafficStatisticService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DayTrafficStatisticService(ILogger<DayTrafficStatisticService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAsync<TField>(Expression<Func<DaysStatistics,TField>> orderBy, int maxCount) 
        {
            return await _unitOfWork.AnalyticsRepository.GetDaysWithTrafficAsync(null, orderBy, maxCount);
        }
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPeriodAsync<TField>(DateTime startPeriod, DateTime endPeriod, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0)
        {
            return await _unitOfWork.AnalyticsRepository.GetDaysWithTrafficAsync(null, orderBy, maxCount);
        }
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtHallAsync<TField>(long placeHallId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0)
        {
            return await _unitOfWork.AnalyticsRepository.GetDaysWithTrafficAsync(obj => obj.HallID==placeHallId, orderBy, maxCount);
        }
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPlaceAsync<TField>(long placeAddressId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0)
        {
            return await _unitOfWork.AnalyticsRepository.GetDaysWithTrafficAsync(obj => obj.Hall.PlaceAddressID == placeAddressId, orderBy, maxCount);
        }
    }
}
