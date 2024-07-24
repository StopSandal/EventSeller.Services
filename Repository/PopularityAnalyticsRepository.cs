using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="IPopularityAnalyticsRepository"/> interface to provide methods for retrieving popularity analytics data.
    /// </summary>
    public class PopularityAnalyticsRepository : IPopularityAnalyticsRepository
    {
        private readonly SellerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopularityAnalyticsRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public PopularityAnalyticsRepository(SellerContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no events are found with sold tickets.</exception>
        public async Task<IEnumerable<EventPopularityStatistic>> GetEventsWithMaxPopularityAsync(Expression<Func<Event, bool>>? eventsFilter = null, Expression<Func<EventPopularityStatistic, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<Event> events = _context.Set<Event>();

            if (eventsFilter != null)
            {
                events.Where(eventsFilter);
            }

            var query = events
                .Select(eventEntity => new
                {
                    EventId = eventEntity.ID,
                    Tickets = eventEntity.EventSessions.SelectMany(x => x.Tickets).AsQueryable().ToList()
                })
                .Where(e => e.Tickets.Any())
                .Select(events => new
                {
                    EventId = events.EventId,
                    TotalTickets = events.Tickets.Count(),
                    TotalSold = events.Tickets.Count(ticket => ticket.isSold),
                    PossibleIncome = events.Tickets.Sum(ticket => ticket.Price),
                    TotalIncome = events.Tickets.Where(ticket => ticket.isSold).Sum(ticket => ticket.Price)

                });


            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null || !eventsWithPopularity.Any())
            {
                throw new InvalidDataException("No seats found.");
            }

            if (orderBy == null)
            {
                orderBy = x => x.PopularityStatistic.Popularity;
            }

            var result = eventsWithPopularity
                .Select(e => new EventPopularityStatistic
                {
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.TotalSold / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.TotalSold,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = ((decimal)e.TotalSold / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    },
                    EventId = e.EventId
                })
                .OrderByDescending(orderBy.Compile());
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no event types are found with sold tickets.</exception>
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<EventType> eventTypes = _context.Set<EventType>();

            if (eventsFilter != null)
            {
                eventTypes = eventTypes.Where(eventsFilter);
            }

            var query = eventTypes
                .Select(eventTypeEntity => new
                {
                    EventType = eventTypeEntity,
                    Tickets = eventTypeEntity.Event.SelectMany(e => e.EventSessions).SelectMany(es => es.Tickets).ToList()
                })
                .Where(e => e.Tickets.Any())
                .Select(e => new
                {
                    EventTypeId = e.EventType.Id,
                    TotalTickets = e.Tickets.Count,
                    SoldCount = e.Tickets.Count(ticket => ticket.isSold),
                    PossibleIncome = e.Tickets.Sum(ticket => ticket.Price),
                    TotalIncome = e.Tickets.Where(ticket => ticket.isSold).Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalTickets > 0);

            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null || !eventsWithPopularity.Any())
            {
                throw new InvalidDataException("No event types found with sold tickets.");
            }

            if (orderBy == null)
            {
                orderBy = x => x.PopularityStatistic.Popularity;
            }

            var resultEntities = eventsWithPopularity
                .Select(e => new EventTypePopularityStatisticDTO
                {
                    EventTypeId = e.EventTypeId,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.SoldCount,
                        Monetization = e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome,
                        Popularity = (decimal)e.SoldCount / e.TotalTickets * (e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome)
                    }
                })
                .OrderByDescending(orderBy.Compile())
                .ToList();

            return resultEntities;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no tickets for seats are found.</exception>
        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<TicketSeat, bool>>? ticketSeatsFilter = null, int maxCount = 0)
        {
            IQueryable<TicketSeat> seats = _context.Set<TicketSeat>();

            if (ticketSeatsFilter != null)
            {
                seats = seats.Where(ticketSeatsFilter);
            }

            var query = seats
                .Select(seat => new
                {
                    TotalTickets = seat.Tickets.Count,
                    TotalSold = seat.Tickets.Count(x => x.isSold),
                    PossibleIncome = seat.Tickets.Sum(x => x.Price),
                    TotalIncome = seat.Tickets.Select(x => new { x.Price, x.isSold }).Where(x => x.isSold).Sum(x => x.Price),
                    SeatId = seat.ID
                })
                .Where(e => e.TotalTickets > 0);

            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var statisticsList = await query.ToListAsync();

            if (statisticsList == null || !statisticsList.Any())
            {
                throw new InvalidDataException("No Tickets for seats found.");
            }

            var resultEntities = statisticsList
                .Select(e => new SeatPopularityDTO
                {
                    SeatId = e.SeatId,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.TotalSold / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.TotalSold,
                        Monetization = e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome,
                        Popularity = (decimal)e.TotalSold / e.TotalTickets * (e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome)
                    }
                })
                .OrderByDescending(orderBy.Compile())
                .ToList();

            return resultEntities;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no seats for the event are found.</exception>
        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityForEventAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<TicketSeat, bool>>? seatsFilter = null, int maxCount = 0)
        {
            IQueryable<TicketSeat> seats = _context.Set<TicketSeat>();

            if (seatsFilter != null)
            {
                seats = seats.Where(seatsFilter);
            }

            var query = seats
                .Select(seat => new
                {
                    Seat = seat,
                    Tickets = seat.Tickets.AsQueryable().Where(ticketsFilter).ToList()
                })
                .Where(seatWithTickets => seatWithTickets.Tickets.Any())
                .Select(seatWithTickets => new
                {
                    SeatId = seatWithTickets.Seat.ID,
                    TotalTickets = seatWithTickets.Tickets.Count(),
                    TotalSold = seatWithTickets.Tickets.Count(ticket => ticket.isSold),
                    PossibleIncome = seatWithTickets.Tickets.Sum(ticket => ticket.Price),
                    TotalIncome = seatWithTickets.Tickets.Where(ticket => ticket.isSold).Sum(ticket => ticket.Price)
                });

            if (maxCount > 0)
            {
                query = query.Take(maxCount);
            }

            var seatsWithPopularity = await query.ToListAsync();

            if (seatsWithPopularity == null || !seatsWithPopularity.Any())
            {
                throw new InvalidDataException("No seats found.");
            }

            return seatsWithPopularity
                .Select(e => new SeatPopularityDTO
                {
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.TotalSold / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.TotalSold,
                        Monetization = e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome,
                        Popularity = (decimal)e.TotalSold / e.TotalTickets * (e.PossibleIncome == 0 ? 0 : e.TotalIncome / e.PossibleIncome)
                    },
                    SeatId = e.SeatId
                })
                .OrderByDescending(orderBy.Compile());
        }

        /// <inheritdoc />
        /// <exception cref="InvalidDataException">Thrown when no sectors for the event are found.</exception>
        public async Task<IEnumerable<EventSeatPopularityDTO>> GetSeatPopularityForEventsGroupsAsync<TField>(Func<SeatPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<TicketSeat, bool>>? seatsFilter = null, int maxCount = 0)
        {
            IQueryable<TicketSeat> seats = _context.Set<TicketSeat>();

            if (seatsFilter != null)
            {
                seats = seats.Where(seatsFilter);
            }

            var query = seats
                .SelectMany(seat => seat.Tickets.AsQueryable().Where(ticketsFilter), (seat, ticket) => new { seat, ticket })
                .GroupBy(st => new { st.seat.ID, st.ticket.EventSession.EventID })
                .Select(g => new
                {
                    SeatId = g.Key.ID,
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
                .Select(g => new EventSeatPopularityDTO
                {
                    EventId = g.Key,
                    SeatPopularity = g.Select(sp => new SeatPopularityDTO
                    {
                        SeatId = sp.SeatId,
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
