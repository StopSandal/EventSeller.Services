using DataLayer.Model;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class BookingService : IBookingService
    {
        private readonly IConfiguration _configuration;
        private readonly ITicketService _ticketService;
        const string TEMPORAL_BOOKING_IN_MINUTES = "Booking:TemporalBookingForPurchaseInMinutes";
        public BookingService(IConfiguration configuration, ITicketService ticketService)
        {
            _configuration = configuration;
            _ticketService = ticketService;
        }

        public bool IsTicketBooked(Ticket ticket)
        {   
            if (ticket.BookedUntil == null) 
                return false;
            if (ticket.BookedUntil <= DateTime.UtcNow)
                return false;
            return true;
        }

        public void TemporaryBookTicketForPurchase(Ticket ticket)
        {
            var minutesForBooking = int.Parse(_configuration[TEMPORAL_BOOKING_IN_MINUTES]);
            ticket.BookedUntil = DateTime.UtcNow.AddMinutes(minutesForBooking);
        }

        public void UnbookTicket(Ticket ticket)
        {
            ticket.BookedUntil = null;
        }

        public async Task UnbookTicketByIdAsync(long ticketId)
        {
            var ticket = await _ticketService.GetByIDAsync(ticketId);
            UnbookTicket(ticket);
        }
    }
}
