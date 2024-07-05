using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Helpers;
using EventSeller.Services.Interfaces.Services;
using System.Linq.Expressions;


namespace EventSeller.Services.Service
{

    public class EventPopularityService : IEventPopularityService
    {
        private readonly ITicketService _ticketService;
        private readonly IEventSessionService _eventSessionService;
        private readonly IEventService _eventService;
        private readonly Expression<Func<Ticket, bool>> ticketSoldFilter = ticket => ticket.isSold;

        private const string TicketEventSessionPropertyInclude = nameof(EventSession);
        private const string TicketEventPropertyInclude = $"{nameof(EventSession)}.{nameof(Event)}";

        public Task<object> GetEventsPopularityByPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            throw new NotImplementedException();
        }

        public async Task<PopularityStatisticDTO> GetEventTypePopularity(long eventTypeId)
        {
            var statistics = await GetTicketsStatisticAsync(ticket => ticket.EventSession.Event.EventTypeID == eventTypeId, [TicketEventSessionPropertyInclude, TicketEventPropertyInclude]);
            return statistics;
            
        }

        public Task<object> GetMostPopularEvent()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetMostRealizableEvent()
        {
            throw new NotImplementedException();
        }
        private async Task<PopularityStatisticDTO> GetTicketsStatisticAsync(Expression<Func<Ticket, bool>> ticketFilter, IEnumerable<string> includes = null)
        {
            var statisticDTO = new PopularityStatisticDTO();

            var totalSold = await _ticketService.GetTicketCountAsync(ticketFilter, includes);

            if(totalSold == 0)
            {
                throw new InvalidDataException("Total sold tickets for that is zero");
            }

            var soldTicketsFilter = ticketFilter.AddAlso(ticket => ticket.isSold);

            var soldCount = await _ticketService.GetTicketCountAsync(soldTicketsFilter, includes);

            statisticDTO.Realization = soldCount / totalSold;

            var getTotalIncome = await _ticketService.GetTicketTotalPriceAsync(soldTicketsFilter, includes);
            
            statisticDTO.Popularity = getTotalIncome * statisticDTO.Realization;

            return statisticDTO;
        }
    }
}
