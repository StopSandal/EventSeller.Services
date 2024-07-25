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
        private ITrafficAnalyticsRepository trafficAnalyticsRepository;
        private IEventPopularityRepository eventPopularityRepository;
        private IEventTypePopularityRepository eventTypePopularityRepository;
        private ISeatPopularityRepository seatPopularityRepository;
        private ISectorPopularityRepository sectorPopularityRepository;
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
                return eventRepository ??= new GenericRepository<Event>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<HallSector> HallSectorRepository
        {
            get
            {
                return hallSectorRepository ??= new GenericRepository<HallSector>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<PlaceAddress> PlaceAddressRepository
        {
            get
            {
                return placeAddressRepository ??= new GenericRepository<PlaceAddress>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<PlaceHall> PlaceHallRepository
        {
            get
            {
                return placeHallRepository ??= new GenericRepository<PlaceHall>(context);
            }
        }

        public IRepositoryAsync<Ticket> TicketRepository
        {
            get
            {
                return ticketRepository ??= new GenericRepository<Ticket>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<TicketSeat> TicketSeatRepository
        {
            get
            {
                return ticketSeatRepository ??= new GenericRepository<TicketSeat>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<TicketTransaction> TicketTransactionRepository
        {
            get
            {
                return ticketTransactionRepository ??= new GenericRepository<TicketTransaction>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<EventType> EventTypeRepository
        {
            get
            {
                return eventTypeRepository ??= new GenericRepository<EventType>(context);
            }
        }

        /// <inheritdoc />
        public IRepositoryAsync<EventSession> EventSessionRepository
        {
            get
            {
                return eventSessionRepository ??= new GenericRepository<EventSession>(context);
            }
        }

        /// <inheritdoc />
        public ISalesAnalyticsRepository SalesAnalyticsRepository
        {
            get
            {
                return salesAnalyticsRepository ??= new SalesAnalyticsRepository(context);
            }
        }

        /// <inheritdoc />
        public ITrafficAnalyticsRepository TrafficAnalyticsRepository
        {
            get
            {
                return trafficAnalyticsRepository ??= new TrafficAnalyticsRepository(context);
            }
        }

        /// <inheritdoc />
        public IEventPopularityRepository EventPopularityRepository
        {
            get
            {
                return eventPopularityRepository ??= new EventPopularityRepository(context);
            }
        }
        /// <inheritdoc />
        public IEventTypePopularityRepository EventTypePopularityRepository
        {
            get
            {
                return eventTypePopularityRepository ??= new EventTypePopularityRepository(context);
            }
        }

        /// <inheritdoc />
        public ISectorPopularityRepository SectorPopularityRepository
        {
            get
            {
                return sectorPopularityRepository ??= new SectorPopularityRepository(context);
            }
        }

        /// <inheritdoc />
        public ISeatPopularityRepository SeatPopularityRepository
        {
            get
            {
                return seatPopularityRepository ??= new SeatPopularityRepository(context);
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
