using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Helpers;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EventSeller.Services.Service
{
    public class TicketSalesStatisticService : ITicketSalesStatisticService
    {
        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;
        private readonly IEventSessionService _eventSessionService;
        private readonly ILogger<TicketSalesStatisticService> _logger;
        private readonly Expression<Func<Ticket, bool>> soldFilter = ticket => ticket.isSold;

        private const string TicketEventSessionPropertyInclude = nameof(EventSession);
        private const string TicketEventPropertyInclude = $"{nameof(EventSession)}.{nameof(Event)}";

        public TicketSalesStatisticService(ITicketService ticketService, IEventService eventService, IEventSessionService eventSessionService, ILogger<TicketSalesStatisticService> logger)
        {
            _ticketService = ticketService;
            _eventService = eventService;
            _eventSessionService = eventSessionService;
            _logger = logger;
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId)
        {
            return await GetTicketsStatisticAsync(ticket => ticket.EventSession.EventID == eventId, [TicketEventSessionPropertyInclude]);
        }
        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAndSessionAsync(long eventId, long eventSessionId)
        {
            return await GetTicketsStatisticAsync(
                ticket => ticket.EventSession.EventID == eventId
                && ticket.EventSessionID == eventSessionId
                , [TicketEventSessionPropertyInclude]);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventsAsync(IEnumerable<long> eventIds)
        {
            return await GetTicketsStatisticAsync(ticket => eventIds.Contains(ticket.EventSession.EventID), [TicketEventSessionPropertyInclude]);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId)
        {
            return await GetTicketsStatisticAsync(ticket => ticket.EventSessionID == eventSessionId);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionsAsync(IEnumerable<long> eventSessionIds)
        {
            return await GetTicketsStatisticAsync(ticket => eventSessionIds.Contains(ticket.EventSessionID));
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId)
        {
            return await GetTicketsStatisticAsync(ticket => ticket.EventSession.Event.EventTypeID == eventTypeId, [TicketEventSessionPropertyInclude, TicketEventPropertyInclude]);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod)
        {
            return await GetTicketsStatisticAsync(
                ticket => ticket.EventSession.StartSessionDateTime > firstPeriod
                && ticket.EventSession.StartSessionDateTime <= secondPeriod
                , [TicketEventSessionPropertyInclude]);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate)
        {
            var weekEndDate = weekStartDate.Date.AddDays(7);
            return await GetSalesStatisticForPeriodAsync(weekStartDate, weekEndDate);
        }
        public async Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate)
        {
            var dayStartTime = dayDate.Date;
            var dayEndTime = dayStartTime.AddDays(1);
            return await GetSalesStatisticForPeriodAsync(dayStartTime, dayEndTime);
        }

        private async Task<SalesStatisticsDTO> GetTicketsStatisticAsync(Expression<Func<Ticket, bool>> ticketFilter, IEnumerable<string> includes = null)
        {
            var statisticDTO = new SalesStatisticsDTO();

            statisticDTO.TotalTickets = await _ticketService.GetTicketCountAsync(ticketFilter, includes);

            var soldTicketsFilter = ticketFilter.AddAlso(ticket => ticket.isSold);

            statisticDTO.Sold = await _ticketService.GetTicketCountAsync(soldTicketsFilter, includes);

            return statisticDTO;
        }
    }
}
