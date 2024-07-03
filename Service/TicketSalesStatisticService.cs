using DataLayer.Model;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class TicketSalesStatisticService : ITicketSalesStatisticService
    {
        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;
        private readonly IEventSessionService _eventSessionService;
        public Task<SalesStatisticsDTO> GetSalesStatisticForDayAsync(DateTime dayDate)
        {
            throw new NotImplementedException();
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(long eventId)
        {
            var eventSessionsIds = await _eventSessionService.GetFieldValuesAsync(obj => obj.EventID==eventId, obj => obj.Id);

            var ticketsIds = await _ticketService.GetFieldValuesAsync(ticket => eventSessionsIds.Contains(ticket.EventSessionID), obj => obj.ID);

            return await GetTicketsStatisticAsync(ticketsIds);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventAsync(IEnumerable<long> eventIds)
        {
            var eventSessionsIds = await _eventSessionService.GetFieldValuesAsync(obj => eventIds.Contains(obj.EventID), obj => obj.Id);

            var ticketsIds = await _ticketService.GetFieldValuesAsync(ticket => eventSessionsIds.Contains(ticket.EventSessionID), obj => obj.ID);

            return await GetTicketsStatisticAsync(ticketsIds);
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForEventSessionAsync(long eventSessionId)
        {
            var ticketsIds = await _ticketService.GetFieldValuesAsync(ticket => ticket.EventSessionID==eventSessionId, obj => obj.ID);

            return await GetTicketsStatisticAsync(ticketsIds);
        }

        public Task<SalesStatisticsDTO> GetSalesStatisticForEventTypeAsync(long eventTypeId)
        {
            throw new NotImplementedException();
        }

        public async Task<SalesStatisticsDTO> GetSalesStatisticForPeriodAsync(DateTime firstPeriod, DateTime secondPeriod)
        {
            //var ticketsIds = await _ticketService.GetFieldValuesAsync(ticket => ticket.)
        }

        public Task<SalesStatisticsDTO> GetSalesStatisticForWeekAsync(DateTime weekStartDate)
        {
            throw new NotImplementedException();
        }
        private async Task<SalesStatisticsDTO> GetTicketsStatisticAsync(IEnumerable<long> ticketsIds) 
        {
            var statisticDTO = new SalesStatisticsDTO();

            statisticDTO.TotalTickets =  ticketsIds.Count();
            statisticDTO.Sold = await _ticketService.GetTicketCountAsync(ticket => ticket.isSold);

            return statisticDTO;

        }
    }
}
