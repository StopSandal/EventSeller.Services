using EventSeller.DataLayer.Entities;


namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Defines methods for managing the booking of tickets.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Asynchronously temporarily books a ticket for purchase.
        /// </summary>
        /// <param name="ticket">The ticket to be temporarily booked.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task TemporaryBookTicketForPurchaseAsync(Ticket ticket);

        /// <summary>
        /// Checks if a ticket is currently booked.
        /// </summary>
        /// <param name="ticket">The ticket to check the booking status of.</param>
        /// <returns>True if the ticket is booked, otherwise false.</returns>
        bool IsTicketBooked(Ticket ticket);

        /// <summary>
        /// Asynchronously unbooks a ticket by its ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to be unbooked.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UnbookTicketByIdAsync(long ticketId);
        public Task PermanentlyBookTicketForPurchaseAsync(Ticket ticket);
    }
}
