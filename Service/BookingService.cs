using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service for managing ticket bookings.
    /// Implementation of the interface <see cref="IBookingService"/>
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IConfiguration _configuration;
        private readonly ITicketService _ticketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingService> _logger;
        const string TEMPORAL_BOOKING_IN_MINUTES = "Booking:TemporalBookingForPurchaseInMinutes";
        /// <summary>
        /// Initializes a new instance of the <see cref="BookingService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="ticketService">The ticket service.</param>
        /// <param name="logger">The logger.</param>
        public BookingService(IConfiguration configuration, ITicketService ticketService, ILogger<BookingService> logger, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _ticketService = ticketService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when the ticket is null.</exception>
        public bool IsTicketBooked(Ticket ticket)
        {
            if (ticket == null)
            {
                _logger.LogError("IsTicketBooked: Ticket is null");
                throw new ArgumentNullException(nameof(ticket));
            }

            if (ticket.BookedUntil == null || ticket.BookedUntil <= DateTime.UtcNow)
                return false;

            return true;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when the ticket is null.</exception>
        /// <exception cref="FormatException">Thrown when the configuration value for booking minutes is invalid.</exception>
        public async Task TemporaryBookTicketForPurchaseAsync(Ticket ticket)
        {
            if (ticket == null)
            {
                _logger.LogError("TemporaryBookTicketForPurchase: Ticket is null");
                throw new ArgumentNullException(nameof(ticket));
            }

            try
            {
                var minutesForBooking = int.Parse(_configuration[TEMPORAL_BOOKING_IN_MINUTES]);
                ticket.BookedUntil = DateTime.UtcNow.AddMinutes(minutesForBooking);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("TemporaryBookTicketForPurchase: Ticket booked until {BookedUntil}", ticket.BookedUntil);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "TemporaryBookTicketForPurchase: Invalid configuration value for booking minutes");
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown when the ticket retrieval fails.</exception>
        public async Task UnbookTicketByIdAsync(long ticketId)
        {
            try
            {
                var ticket = await _ticketService.GetByIDAsync(ticketId);

                if (ticket == null)
                {
                    _logger.LogError("UnbookTicket: Ticket is null");
                    throw new ArgumentNullException(nameof(ticket));
                }

                ticket.BookedUntil = null;
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("UnbookTicketByIdAsync: Ticket with ID {TicketId} unbooked", ticketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UnbookTicketByIdAsync: Error unbooking ticket with ID {TicketId}", ticketId);
                throw;
            }
        }
    }
}
