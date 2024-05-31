using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<Event> EventRepository { get; }
        IRepositoryAsync<HallSector> HallSectorRepository { get; }
        IRepositoryAsync<PlaceAddress> PlaceAddressRepository { get; }
        IRepositoryAsync<PlaceHall> PlaceHallRepository { get; }
        IRepositoryAsync<Ticket> TicketRepository { get; }
        IRepositoryAsync<TicketSeat> TicketSeatRepository { get; }
        Task Save();
    }
}
