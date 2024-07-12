using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    public interface ITrafficAnalyticsRepository
    {
        public Task<IEnumerable<DaysStatistics>> GetDaysWithTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, Expression<Func<EventSession, bool>>? eventsFilter = null, int maxCount = 0);
    }
}
