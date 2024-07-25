using EventSeller.DataLayer.Entities;
using EventSeller.Services.Interfaces.Repositories;

namespace EventSeller.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for the unit of work, which includes repository properties and a save method.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for <see cref="Event"/> entities.
        /// </summary>
        IRepositoryAsync<Event> EventRepository { get; }

        /// <summary>
        /// Gets the repository for <see cref="HallSector"/> entities.
        /// </summary>
        IRepositoryAsync<HallSector> HallSectorRepository { get; }

        /// <summary>
        /// Gets the repository for <see cref="PlaceAddress"/> entities.
        /// </summary>
        IRepositoryAsync<PlaceAddress> PlaceAddressRepository { get; }

        /// <summary>
        /// Gets the repository for <see cref="PlaceHall"/> entities.
        /// </summary>
        IRepositoryAsync<PlaceHall> PlaceHallRepository { get; }

        /// <summary>
        /// Gets the repository for <see cref="Ticket"/> entities.
        /// </summary>
        IRepositoryAsync<Ticket> TicketRepository { get; }
        /// <summary>
        /// Gets the repository for <see cref="TicketSeat"/> entities.
        /// </summary>
        IRepositoryAsync<TicketSeat> TicketSeatRepository { get; }
        /// <summary>
        /// Gets the repository for <see cref="TicketSeat"/> entities.
        /// </summary>
        IRepositoryAsync<TicketTransaction> TicketTransactionRepository { get; }
        /// <summary>
        /// Gets the repository for <see cref="EventType"/> entities.
        /// </summary>
        IRepositoryAsync<EventType> EventTypeRepository { get; }
        /// <summary>
        /// Gets the repository for <see cref="EventSession"/> entities.
        /// </summary>
        IRepositoryAsync<EventSession> EventSessionRepository { get; }
        /// <summary>
        /// Gets the repository for traffic analytics.
        /// </summary>
        ITrafficAnalyticsRepository TrafficAnalyticsRepository { get; }

        /// <summary>
        /// Gets the repository for sales analytics.
        /// </summary>
        ISalesAnalyticsRepository SalesAnalyticsRepository { get; }

        /// <summary>
        /// Gets the repository for event popularity.
        /// </summary>
        IEventPopularityRepository EventPopularityRepository { get; }

        /// <summary>
        /// Gets the repository for event type popularity.
        /// </summary>
        IEventTypePopularityRepository EventTypePopularityRepository { get; }

        /// <summary>
        /// Gets the repository for sector popularity.
        /// </summary>
        ISectorPopularityRepository SectorPopularityRepository { get; }

        /// <summary>
        /// Gets the repository for seat popularity.
        /// </summary>
        ISeatPopularityRepository SeatPopularityRepository { get; }

        /// <summary>
        /// Saves all changes made in this unit of work to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveAsync();
    }
}
