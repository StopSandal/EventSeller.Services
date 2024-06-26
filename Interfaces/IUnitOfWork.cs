using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Saves all changes made in this unit of work to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveAsync();
    }
}
