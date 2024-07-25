using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="ISectorPopularityRepository"/> interface to provide methods for retrieving popularity data for <see cref="HallSector"/>.
    /// </summary>
    public class SectorPopularityRepository : ISectorPopularityRepository
    {
        private readonly SellerContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="SectorPopularityRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SectorPopularityRepository(SellerContext context)
        {
            _context = context;
        }
        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no sectors are found.</exception>
        public async Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0)
        {
            IQueryable<HallSector> sectors = _context.Set<HallSector>();

            if (sectorsFilter != null)
            {
                sectors = sectors.Where(sectorsFilter);
            }

            var query = sectors
                 .Select(sector => new
                 {
                     Sector = sector,
                     Tickets = sector.TicketSeats.SelectMany(x => x.Tickets).AsQueryable().ToList()
                 })
                .Select(_ => new
                {
                    TotalTickets = _.Tickets.Count(),
                    TotalSold = _.Tickets.Count(x => x.isSold),
                    PossibleIncome = _.Tickets.Sum(x => x.Price),
                    TotalIncome = _.Tickets.Where(x => x.isSold).Sum(x => x.Price),
                    SectorId = _.Sector.ID
                })
                .Where(e => e.TotalTickets > 0);


            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var statisticsList = await query.ToListAsync();

            if (statisticsList == null)
            {
                throw new InvalidDataException("No Tickets for sector found.");
            }

            return statisticsList
                            .Select(e => new SectorPopularityDTO
                            {
                                PopularityStatistic = new PopularityStatisticDTO
                                {
                                    Realization = (decimal)e.TotalSold / e.TotalTickets,
                                    TotalIncome = e.TotalIncome,
                                    TotalSold = e.TotalSold,
                                    Monetization = e.TotalIncome / e.PossibleIncome,
                                    Popularity = (e.TotalSold / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                                },
                                SectorId = e.SectorId
                            })
                            .OrderByDescending(orderBy.Compile());
        }
        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no sectors for the event are found.</exception>
        public async Task<IEnumerable<SectorPopularityDTO>> GetSectorsPopularityForEventAsync<TField>(Expression<Func<SectorPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0)
        {
            IQueryable<HallSector> sectors = _context.Set<HallSector>();

            if (sectorsFilter != null)
            {
                sectors = sectors.Where(sectorsFilter);
            }

            var query = sectors
                .Select(sector => new
                {
                    Sector = sector,
                    Tickets = sector.TicketSeats.SelectMany(x => x.Tickets).AsQueryable().Where(ticketsFilter).ToList()
                })
                .Where(sectorWithTickets => sectorWithTickets.Tickets.Any())
                .Select(sectorWithTickets => new
                {
                    SectorId = sectorWithTickets.Sector.ID,
                    TotalTickets = sectorWithTickets.Tickets.Count(),
                    TotalSold = sectorWithTickets.Tickets.Count(ticket => ticket.isSold),
                    PossibleIncome = sectorWithTickets.Tickets.Sum(ticket => ticket.Price),
                    TotalIncome = sectorWithTickets.Tickets.Where(ticket => ticket.isSold).Sum(ticket => ticket.Price)
                });

            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var sectorsWithPopularity = await query.ToListAsync();

            if (sectorsWithPopularity == null || !sectorsWithPopularity.Any())
            {
                throw new InvalidDataException("No seats found.");
            }

            return sectorsWithPopularity
                .Select(e => new SectorPopularityDTO
                {
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.TotalSold / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.TotalSold,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = ((decimal)e.TotalSold / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    },
                    SectorId = e.SectorId
                })
                .OrderByDescending(orderBy.Compile());
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no sectors for the event are found.</exception>
        public async Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityForEventsGroupsAsync<TField>(Func<SectorPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0)
        {
            IQueryable<HallSector> sectors = _context.Set<HallSector>();

            if (sectorsFilter != null)
            {
                sectors = sectors.Where(sectorsFilter);
            }

            var query = sectors
                .SelectMany(sector => sector.TicketSeats.SelectMany(x => x.Tickets).AsQueryable().Where(ticketsFilter)
                    , (sector, ticket) => new { sector, ticket })
                .GroupBy(st => new { st.sector.ID, st.ticket.EventSession.EventID })
                .Select(g => new
                {
                    SectorId = g.Key.ID,
                    EventId = g.Key.EventID,
                    TotalTickets = g.Count(),
                    TotalSold = g.Count(st => st.ticket.isSold),
                    PossibleIncome = g.Sum(st => st.ticket.Price),
                    TotalIncome = g.Where(st => st.ticket.isSold).Sum(st => st.ticket.Price)
                })
                .Where(e => e.TotalTickets > 0);

            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var finalResult = await query.ToListAsync();

            var resultEntities = finalResult
                .GroupBy(sp => sp.EventId)
                .Select(g => new EventSectorPopularityDTO
                {
                    EventId = g.Key,
                    SectorPopularity = g.Select(sp => new SectorPopularityDTO
                    {
                        SectorId = sp.SectorId,
                        PopularityStatistic = new PopularityStatisticDTO
                        {
                            TotalSold = sp.TotalSold,
                            TotalIncome = sp.TotalIncome,
                            Realization = (decimal)sp.TotalSold / sp.TotalTickets,
                            Monetization = sp.PossibleIncome == 0 ? 0 : sp.TotalIncome / sp.PossibleIncome,
                            Popularity = (decimal)sp.TotalSold / sp.TotalTickets * (sp.PossibleIncome == 0 ? 0 : sp.TotalIncome / sp.PossibleIncome)
                        }
                    }).OrderByDescending(orderBy).ToList()
                }).ToList();

            return resultEntities;
        }
    }
}
