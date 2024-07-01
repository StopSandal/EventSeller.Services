using DataLayer.Model;
using EventSeller.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class TicketRegistrationService : ITicketRegistrationService
    {
        public Task<IEnumerable<Ticket>> AddTicketsForPlaceHallByCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> AddTicketsForPlaceHallForAllSeatsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
