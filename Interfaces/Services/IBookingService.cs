using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface IBookingService
    {
        public void TemporaryBookTicketForPurchase(Ticket ticket);
        public bool IsTicketBooked(Ticket ticket);
        public void UnbookTicket(Ticket ticket);
        public Task UnbookTicketByIdAsync(long ticketId);

    }
}
