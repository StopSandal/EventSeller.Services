using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface IDayTrafficStatisticService
    {
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, int maxCount);
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPeriodAsync<TField>(DateTime startPeriod, DateTime endPeriod, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0);
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtHallAsync<TField>(long placeHallId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0);
        Task<IEnumerable<DaysStatistics>> GetDaysTrafficAtPlaceAsync<TField>(long placeAddressId, Expression<Func<DaysStatistics, TField>> orderBy, int maxCount = 0);
    }
}
