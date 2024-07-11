using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace EventSeller.Services.Repository
{
    internal class AnalyticsRepositoryAsync : IAnalyticsRepositoryAsync
    {
        private readonly SellerContext _context;

        public AnalyticsRepositoryAsync(SellerContext context)
        {
            _context = context;
        }

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
                    Event = eventEntity,
                    TotalTickets = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Count(),
                    SoldCount = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Count(ticket => ticket.isSold),
                    PossibleIncome = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Sum(ticket => ticket.Price),
                    TotalIncome = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Where(ticket => ticket.isSold)
                        .Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalTickets > 0)
                .Select(e => new EventPopularityStatistic
                {
                    EventItem = e.Event,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.SoldCount,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = (e.SoldCount / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    }
                });

            if (orderBy != null)
            {
                query.OrderByDescending(orderBy);
            }

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No events found with sold tickets.");
            }

            return eventsWithPopularity;
        }
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<EventType> eventTypes = _context.Set<EventType>();

            if (eventsFilter != null)
            {
                eventTypes.Where(eventsFilter);
            }
            var query = eventTypes
                .Select(eventTypeEntity => new
                {
                    EventType = eventTypeEntity,
                    TotalTickets = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(),
                    SoldCount = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(ticket => ticket.isSold),
                    PossibleIncome = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Sum(ticket => ticket.Price),
                    TotalIncome = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Where(ticket => ticket.isSold)
                        .Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalTickets > 0)
                .Select(e => new EventTypePopularityStatisticDTO
                {
                    EventTypeItem = e.EventType,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.SoldCount,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = (e.SoldCount / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    }
                });

            if (orderBy != null)
            {
                query.OrderByDescending(orderBy);
            }

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No event types found with sold tickets.");
            }

            return eventsWithPopularity;
        }
        public async Task<EventTypePopularityStatisticDTO> GetEventTypeWithMaxPopularityAsync(Expression<Func<EventType, bool>> eventTypeFilter)
        {
            var events = _context.Set<EventType>()
                            .Where(eventTypeFilter);

            var eventsWithPopularity = await events
                .Select(eventTypeEntity => new
                {
                    EventType = eventTypeEntity,
                    TotalTickets = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(),
                    SoldCount = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(ticket => ticket.isSold),
                    PossibleIncome = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Sum(ticket => ticket.Price),
                    TotalIncome = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Where(ticket => ticket.isSold)
                        .Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalTickets > 0)
                .Select(e => new EventTypePopularityStatisticDTO
                {
                    EventTypeItem = e.EventType,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.SoldCount,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = (e.SoldCount / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    }
                })
                .FirstOrDefaultAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No events found with sold tickets.");
            }

            return eventsWithPopularity;
        }
        public async Task<IEnumerable<DaysStatistics>> GetDaysWithTrafficAsync<TField>(Expression<Func<DaysStatistics, TField>> orderBy, Expression<Func<EventSession, bool>>? eventsFilter = null, int maxCount = 0)
        {
            IQueryable<EventSession> eventSessions = _context.Set<EventSession>();

            if (eventsFilter != null)
            {
                eventSessions = eventSessions.Where(eventsFilter);
            }

            var query = eventSessions
                .SelectMany(item => item.Tickets.Select(ticket => ticket.isSold).Where(x => x), (item, ticket) => new { item, ticket })
                .GroupBy(x => x.item.StartSessionDateTime.Date)
                .Select(g => new DaysStatistics
                {
                    Date = g.Key,
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    TotalTraffic = g.Count()
                });

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var eventsWithPopularity = await query.OrderByDescending(orderBy).ToListAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No days found.");
            }

            return eventsWithPopularity;
        }
        public async Task<IEnumerable<SeatPopularityDTO>> GetSeatPopularityAsync<TField>(Expression<Func<SeatPopularityDTO, TField>> orderBy, Expression<Func<TicketSeat, bool>>? ticketSeatsFilter = null, int maxCount = 0)
        {
            IQueryable<TicketSeat> seats = _context.Set<TicketSeat>();

            if (ticketSeatsFilter != null)
            {
                seats = seats.Where(ticketSeatsFilter);
            }

            var query = seats
                .Select(_ => new
                {
                    TotalTickets = _.Tickets.Count(),
                    TotalSold = _.Tickets.Count(x => x.isSold),
                    PossibleIncome = _.Tickets.Sum(x => x.Price),
                    TotalIncome = _.Tickets.Where(x => x.isSold).Sum(x => x.Price),
                    SeatId = _.ID
                })
                .Where(e => e.TotalTickets > 0)
                .Select(e => new SeatPopularityDTO
                {
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.TotalSold / e.TotalTickets,
                        TotalIncome = e.TotalIncome,
                        TotalSold = e.TotalSold,
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = ((decimal)e.TotalSold / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    },
                    SeatId = e.SeatId
                });

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var statisticsList = await query.OrderByDescending(orderBy).ToListAsync();

            if (statisticsList == null)
            {
                throw new InvalidDataException("No Tickets for seats found.");
            }

            return statisticsList;
        }
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
                        Monetization = e.TotalIncome / e.PossibleIncome,
                        Popularity = ((decimal)e.TotalSold / e.TotalTickets) * (e.TotalIncome / e.PossibleIncome)
                    },
                    SeatId = e.SeatId
                })
                .OrderByDescending(orderBy.Compile());
        }
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
                .Where(e => e.TotalTickets > 0)
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
                });

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var statisticsList = await query.OrderByDescending(orderBy).ToListAsync();

            if (statisticsList == null)
            {
                throw new InvalidDataException("No Tickets for sector found.");
            }

            return statisticsList;
        }
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
        public async Task<IEnumerable<EventSectorPopularityDTO>> GetSectorsPopularityForEventsGroupsAsync<TField>(Func<SectorPopularityDTO, TField> orderBy, Expression<Func<Ticket, bool>> ticketsFilter, Expression<Func<HallSector, bool>>? sectorsFilter = null, int maxCount = 0)
        {
            IQueryable<HallSector> sectors = _context.Set<HallSector>();

            if (sectorsFilter != null)
            {
                sectors = sectors.Where(sectorsFilter);
            }

            var query = sectors
                .SelectMany(sector => sector.TicketSeats.SelectMany(x => x.Tickets).AsQueryable().Where(ticketsFilter), (sector, ticket) => new { sector, ticket })
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
