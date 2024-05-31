using DataLayer.Model;
using DataLayer.Model.EF;
using EventSeller.Services.Interfaces;
using Services.Repository;


namespace Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SellerContext context;
        private IRepositoryAsync<Event> eventRepository;
        private IRepositoryAsync<HallSector> hallSectorRepository;
        private IRepositoryAsync<PlaceAddress> placeAddressRepository;
        private IRepositoryAsync<PlaceHall> placeHallRepository;
        private IRepositoryAsync<Ticket> ticketRepository;
        private IRepositoryAsync<TicketSeat> ticketSeatRepository;
        public UnitOfWork(SellerContext sellerContext)
        {
            context=sellerContext;
        }

        public IRepositoryAsync<Event> EventRepository
        {
            get
            {
                if (this.eventRepository == null)
                {
                    this.eventRepository = new GenericRepository<Event>(context);
                }
                return eventRepository;
            }
        }
        public IRepositoryAsync<HallSector> HallSectorRepository
        {
            get
            {
                if (this.hallSectorRepository == null)
                {
                    this.hallSectorRepository = new GenericRepository<HallSector>(context);
                }
                return hallSectorRepository;
            }
        }
        public IRepositoryAsync<PlaceAddress> PlaceAddressRepository
        {
            get
            {
                if (this.placeAddressRepository == null)
                {
                    this.placeAddressRepository = new GenericRepository<PlaceAddress>(context);
                }
                return placeAddressRepository;
            }
        }
        public IRepositoryAsync<PlaceHall> PlaceHallRepository
        {
            get
            {
                if (this.placeHallRepository == null)
                {
                    this.placeHallRepository = new GenericRepository<PlaceHall>(context);
                }
                return placeHallRepository;
            }
        }
        public IRepositoryAsync<Ticket> TicketRepository
        {
            get
            {
                if (this.ticketRepository == null)
                {
                    this.ticketRepository = new GenericRepository<Ticket>(context);
                }
                return ticketRepository;
            }
        }
        public IRepositoryAsync<TicketSeat> TicketSeatRepository
        {
            get
            {
                if (this.ticketSeatRepository == null)
                {
                    this.ticketSeatRepository = new GenericRepository<TicketSeat>(context);
                }
                return ticketSeatRepository;
            }
        }
        public Task Save()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
