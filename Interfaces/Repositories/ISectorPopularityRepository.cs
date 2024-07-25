using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for retrieving popularity data for sector.
    /// </summary>
    public interface ISectorPopularityRepository
    {
        /// <summary>
        /// Retrieves hall sector popularity statistics, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="sectorsFilter">An optional filter to apply to hall sectors.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sector popularity statistics.</returns>
        Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves hall sector popularity statistics for a specific event, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The expression used to order the results.</param>
        /// <param name="ticketsFilter">The filter to apply to tickets related to the event.</param>
        /// <param name="sectorsFilter">An optional filter to apply to hall sectors.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sector popularity statistics.</returns>
        Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);

        /// <summary>
        /// Retrieves hall sector popularity statistics for groups of events, optionally filtered and ordered.
        /// </summary>
        /// <typeparam name="TField">The type of the field used for ordering the results.</typeparam>
        /// <param name="orderBy">The function used to order the results.</param>
        /// <param name="ticketsFilter">The filter to apply to tickets related to the events.</param>
        /// <param name="sectorsFilter">An optional filter to apply to hall sectors.</param>
        /// <param name="maxCount">The maximum number of results to return. If 0, all results are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of hall sector popularity statistics grouped by events.</returns>
        Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityForEventsGroupsAsync<TField>(Func<SectorPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0);

    }
}
