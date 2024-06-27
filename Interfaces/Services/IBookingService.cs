using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Defines methods for managing the booking of tickets.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Temporarily books a ticket for purchase.
        /// </summary>
        /// <param name="ticket">The ticket to be temporarily booked.</param>
        void TemporaryBookTicketForPurchase(Ticket ticket);

        /// <summary>
        /// Checks if a ticket is currently booked.
        /// </summary>
        /// <param name="ticket">The ticket to check the booking status of.</param>
        /// <returns>True if the ticket is booked, otherwise false.</returns>
        bool IsTicketBooked(Ticket ticket);

        /// <summary>
        /// Unbooks a previously booked ticket.
        /// </summary>
        /// <param name="ticket">The ticket to be unbooked.</param>
        void UnbookTicket(Ticket ticket);

        /// <summary>
        /// Asynchronously unbooks a ticket by its ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to be unbooked.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UnbookTicketByIdAsync(long ticketId);
    }
}
