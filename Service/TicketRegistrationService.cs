using DataLayer.Model;
using EventSeller.DataLayer.EntitiesDto.Ticket;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class TicketRegistrationService : ITicketRegistrationService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlaceHallService _placeHallService;
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;
        public async Task<IEnumerable<Ticket>> AddTicketsForPlaceHallByCountAsync(AddTicketsForHallByCountDTO addTicketsForHallByCountDTO)
        {
            if(addTicketsForHallByCountDTO.TicketsCount <= 0)
            {
                throw new InvalidOperationException("Ticket count cannot be lesser than 0");
            }
            if(! await _eventService.DoesExistsByIdAsync(addTicketsForHallByCountDTO.EventID))
            {
                throw new InvalidOperationException("Hall doesn't exists");
            }
            if (!await _placeHallService.DoesExistsByIdAsync(addTicketsForHallByCountDTO.EventID))
            {
                throw new InvalidOperationException("Event doesn't exists");
            }
            var name = addTicketsForHallByCountDTO.Name;
            var description = addTicketsForHallByCountDTO.Description;
            var price = addTicketsForHallByCountDTO.Price;
            var currencyType = addTicketsForHallByCountDTO.CurrencyType;
            var ticketStartDateTime = addTicketsForHallByCountDTO.TicketStartDateTime;
            var ticketEndDateTime = addTicketsForHallByCountDTO.TicketEndDateTime;
            var hallID = addTicketsForHallByCountDTO.HallID;
            var eventID = addTicketsForHallByCountDTO.EventID;

            var ticketList = Enumerable.Range(0, addTicketsForHallByCountDTO.TicketsCount)
                                       .Select(_ => new Ticket
                                       {
                                           Name = name,
                                           Description = description,
                                           Price = price,
                                           CurrencyType = currencyType,
                                           TicketStartDateTime = ticketStartDateTime,
                                           TicketEndDateTime = ticketEndDateTime,
                                           HallID = hallID,
                                           EventID = eventID,
                                       })
                                       .ToList();

            await _ticketService.AddTicketListAsync(ticketList);
            return ticketList;
        }

        public Task<IEnumerable<Ticket>> AddTicketsForPlaceHallForAllSeatsAsync(AddTicketsForHallToFillDTO addTicketsForHallToFillDTO)
        {
            throw new NotImplementedException();
        }
    }
}
