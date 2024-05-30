using DataLayer.Model;
using DataLayer.Model.EF;
using Services.Repository;


namespace Services
{
    public class UnitOfWork : IDisposable
    {
        private readonly SellerContext context;
        private GenericRepository<Event> eventRepository;
        private GenericRepository<HallSector> hallSectorRepository;
        private GenericRepository<PlaceAddress> placeAddressRepository;
        private GenericRepository<PlaceHall> placeHallRepository;
        private GenericRepository<Ticket> ticketRepository;
        private GenericRepository<TicketSeat> ticketSeatRepository;
        public UnitOfWork(SellerContext sellerContext)
        {
            context=sellerContext;
        }

        public GenericRepository<Event> EventRepository
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
        public GenericRepository<HallSector> HallSectorRepository
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
        public GenericRepository<PlaceAddress> PlaceAddressRepository
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
        public GenericRepository<PlaceHall> PlaceHallRepository
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
        public GenericRepository<Ticket> TicketRepository
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
        public GenericRepository<TicketSeat> TicketSeatRepository
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
        public void Save()
        {
            context.SaveChanges();
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
