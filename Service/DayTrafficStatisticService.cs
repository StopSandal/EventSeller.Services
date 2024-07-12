using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Implements the <see cref="IDayTrafficStatisticService"/> interface to provide methods for retrieving day traffic statistics.
    /// </summary>
    public class DayTrafficStatisticService : IDayTrafficStatisticService
    {
        private readonly ILogger<DayTrafficStatisticService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTrafficStatisticService"/> class with the specified logger and unit of work.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DayTrafficStatisticService(ILogger<DayTrafficStatisticService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, int maxCount)
        {
            try
            {
                _logger.LogInformation("Fetching day traffic statistics.");
                return await _unitOfWork.TrafficAnalyticsRepository.GetDaysWithTrafficAsync(orderBy, null, maxCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching day traffic statistics.");
                throw;  
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPeriodAsync<TField>(DateTime startPeriod, DateTime endPeriod, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0)
        {
            try
            {
                _logger.LogInformation("Fetching day traffic statistics for period: {StartPeriod} - {EndPeriod}", startPeriod, endPeriod);
                Expression<Func<EventSession, bool>> dateFilter = obj => obj.StartSessionDateTime >= startPeriod && obj.StartSessionDateTime < endPeriod;
                return await _unitOfWork.TrafficAnalyticsRepository.GetDaysWithTrafficAsync(orderBy, dateFilter, maxCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching day traffic statistics for period: {StartPeriod} - {EndPeriod}", startPeriod, endPeriod);
                throw;  
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtHallAsync<TField>(long placeHallId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0)
        {
            try
            {
                _logger.LogInformation("Fetching day traffic statistics for hall with ID: {PlaceHallId}", placeHallId);
                return await _unitOfWork.TrafficAnalyticsRepository.GetDaysWithTrafficAsync(orderBy, obj => obj.HallID == placeHallId, maxCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching day traffic statistics for hall with ID: {PlaceHallId}", placeHallId);
                throw;  
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPlaceAsync<TField>(long placeAddressId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0)
        {
            try
            {
                _logger.LogInformation("Fetching day traffic statistics for place with Address ID: {PlaceAddressId}", placeAddressId);
                return await _unitOfWork.TrafficAnalyticsRepository.GetDaysWithTrafficAsync(orderBy, obj => obj.Hall.PlaceAddressID == placeAddressId, maxCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching day traffic statistics for place with Address ID: {PlaceAddressId}", placeAddressId);
                throw;  
            }
        }
    }
}
