using EventSeller.DataLayer.EF;
using EventSeller.DataLayer.Entities;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Repositories;
using EventSeller.Services.Repository;


namespace EventSeller.Services
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
        private IPopularityAnalyticsRepository popularityAnalyticsRepository;
        private ITrafficAnalyticsRepository trafficAnalyticsRepository;
        private ISalesAnalyticsRepository salesAnalyticsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified context.
        /// </summary>
        /// <param name="sellerContext">The database context.</param>
        public UnitOfWork(SellerContext sellerContext)
        {
            context = sellerContext;
        }
        /// <inheritdoc />
        public IRepositoryAsync<Event> EventRepository
        {
            get
            {
                if (eventRepository == null)
                {
                    eventRepository = new GenericRepository<Event>(context);
                }
                return eventRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<HallSector> HallSectorRepository
        {
            get
            {
                if (hallSectorRepository == null)
                {
                    hallSectorRepository = new GenericRepository<HallSector>(context);
                }
                return hallSectorRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<PlaceAddress> PlaceAddressRepository
        {
            get
            {
                if (placeAddressRepository == null)
                {
                    placeAddressRepository = new GenericRepository<PlaceAddress>(context);
                }
                return placeAddressRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<PlaceHall> PlaceHallRepository
        {
            get
            {
                if (placeHallRepository == null)
                {
                    placeHallRepository = new GenericRepository<PlaceHall>(context);
                }
                return placeHallRepository;
            }
        }
        public IRepositoryAsync<Ticket> TicketRepository
        {
            get
            {
                if (ticketRepository == null)
                {
                    ticketRepository = new GenericRepository<Ticket>(context);
                }
                return ticketRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<TicketSeat> TicketSeatRepository
        {
            get
            {
                if (ticketSeatRepository == null)
                {
                    ticketSeatRepository = new GenericRepository<TicketSeat>(context);
                }
                return ticketSeatRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<TicketTransaction> TicketTransactionRepository
        {
            get
            {
                if (ticketTransactionRepository == null)
                {
                    ticketTransactionRepository = new GenericRepository<TicketTransaction>(context);
                }
                return ticketTransactionRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<EventType> EventTypeRepository
        {
            get
            {
                if (eventTypeRepository == null)
                {
                    eventTypeRepository = new GenericRepository<EventType>(context);
                }
                return eventTypeRepository;
            }
        }
        /// <inheritdoc />
        public IRepositoryAsync<EventSession> EventSessionRepository
        {
            get
            {
                if (eventSessionRepository == null)
                {
                    eventSessionRepository = new GenericRepository<EventSession>(context);
                }
                return eventSessionRepository;
            }
        }
        /// <inheritdoc />
        public ISalesAnalyticsRepository SalesAnalyticsRepository
        {
            get
            {
                if (salesAnalyticsRepository == null)
                {
                    salesAnalyticsRepository = new SalesAnalyticsRepository(context);
                }
                return salesAnalyticsRepository;
            }
        }
        /// <inheritdoc />
        public ITrafficAnalyticsRepository TrafficAnalyticsRepository
        {
            get
            {
                if (trafficAnalyticsRepository == null)
                {
                    trafficAnalyticsRepository = new TrafficAnalyticsRepository(context);
                }
                return trafficAnalyticsRepository;
            }
        }
        /// <inheritdoc />
        public IPopularityAnalyticsRepository PopularityAnalyticsRepository
        {
            get
            {
                if (popularityAnalyticsRepository == null)
                {
                    popularityAnalyticsRepository = new PopularityAnalyticsRepository(context);
                }
                return popularityAnalyticsRepository;
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
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
