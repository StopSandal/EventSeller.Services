using DataLayer.Model;
using DataLayer.Model.EF;
using EventSeller.Services.Interfaces;
using Services.Repository;


namespace Services
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/>.
    /// </summary>
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SellerContext context;
        private IRepositoryAsync<Event> eventRepository;
        private IRepositoryAsync<HallSector> hallSectorRepository;
        private IRepositoryAsync<PlaceAddress> placeAddressRepository;
        private IRepositoryAsync<PlaceHall> placeHallRepository;
        private IRepositoryAsync<Ticket> ticketRepository;
        private IRepositoryAsync<TicketSeat> ticketSeatRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified context.
        /// </summary>
        /// <param name="sellerContext">The database context.</param>
        public UnitOfWork(SellerContext sellerContext)
        {
            context=sellerContext;
        }
        /// <inheritdoc />
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
        /// <inheritdoc />
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
        /// <inheritdoc />
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
        /// <inheritdoc />
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
        /// <inheritdoc />
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
        
        /// <inheritdoc/>
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
