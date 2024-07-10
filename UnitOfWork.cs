using EventSeller.DataLayer.Entities;
using DataLayer.Model.EF;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Repository;
using Services.Repository;
using EventSeller.Services.Interfaces.Services;


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
        private IRepositoryAsync<EventType> eventTypeRepository;
        private IRepositoryAsync<EventSession> eventSessionRepository;
        private IRepositoryAsync<HallSector> hallSectorRepository;
        private IRepositoryAsync<PlaceAddress> placeAddressRepository;
        private IRepositoryAsync<PlaceHall> placeHallRepository;
        private IRepositoryAsync<Ticket> ticketRepository;
        private IRepositoryAsync<TicketSeat> ticketSeatRepository;
        private IRepositoryAsync<TicketTransaction> ticketTransactionRepository;
        private IAnalyticsRepositoryAsync analyticsRepository;
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
        /// <inheritdoc />
        public IRepositoryAsync<TicketTransaction> TicketTransactionRepository
        {
            get
            {
                if (this.ticketTransactionRepository == null)
                {
                    this.ticketTransactionRepository = new GenericRepository<TicketTransaction>(context);
                }
                return ticketTransactionRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<EventType> EventTypeRepository
        {
            get
            {
                if (this.eventTypeRepository == null)
                {
                    this.eventTypeRepository = new GenericRepository<EventType>(context);
                }
                return eventTypeRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<EventSession> EventSessionRepository
        {
            get
            {
                if (this.eventSessionRepository == null)
                {
                    this.eventSessionRepository = new GenericRepository<EventSession>(context);
                }
                return eventSessionRepository;
            }
        }
        /// <inheritdoc />
        public IAnalyticsRepositoryAsync AnalyticsRepository
        {
            get
            {
                if (this.analyticsRepository == null)
                {
                    this.analyticsRepository = new AnalyticsRepositoryAsync(context);
                }
                return analyticsRepository;
            }
        }

        /// <inheritdoc/>
        public Task SaveAsync()
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
