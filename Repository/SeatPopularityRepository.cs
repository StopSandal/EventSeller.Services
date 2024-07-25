using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// Implements the <see cref="ISeatPopularityRepository"/> interface to provide methods for retrieving popularity data for <see cref="TicketSeat"/>.
    /// </summary>
    public class SeatPopularityRepository : ISeatPopularityRepository
    {
        private readonly SellerContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatPopularityRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SeatPopularityRepository(SellerContext context)
        {
            _context = context;
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
    }
}
