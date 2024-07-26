
using EventSeller.DataLayer.Entities;
using EventSeller.Services.Helpers.Constants;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

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
        private readonly ITimerManager<long> _timerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="ticketService">The ticket service.</param>
        /// <param name="logger">The logger.</param>
        public BookingService(IConfiguration configuration, ITicketService ticketService, ILogger<BookingService> logger, IUnitOfWork unitOfWork, ITimerManager<long> timerService)
        {
            _configuration = configuration;
            _ticketService = ticketService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _timerService = timerService;
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
                var minutesForBooking = int.Parse(_configuration[ConfigurationPathConstants.TemporalBookingInMinutes]);
                ticket.BookedUntil = DateTime.UtcNow.AddMinutes(minutesForBooking);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("TemporaryBookTicketForPurchase: Ticket booked until {BookedUntil}", ticket.BookedUntil);

                _timerService.RegisterTimer<IBookingService>(ticket.ID, service => service.UnbookTicketByIdAsync(ticket.ID), minutesForBooking);
                _logger.LogInformation("TemporaryBookTicketForPurchase: Started unbooking timer for ticket with ID {TicketId}.",ticket.ID);
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
            _logger.LogInformation("UnbookTicketByIdAsync: Started Unbooking ticket {TicketID}", ticketId);
            try
            {
                //await _timerService.CancelTimerAsync<BookingService>(ticketId);
                _logger.LogInformation("UnbookTicketByIdAsync: Check existing timer {TicketID}", ticketId);

                var ticket = await _ticketService.GetByIDAsync(ticketId);
                _logger.LogInformation("UnbookTicketByIdAsync: Got Ticket {TicketID}", ticketId);

                if (ticket == null)
                {
                    _logger.LogError("UnbookTicket: Ticket is null");
                    throw new ArgumentNullException(nameof(ticket));
                }

                if (ticket.BookedUntil == null) 
                {
                    _logger.LogWarning("UnbookTicketByIdAsync: Ticket with ID {TicketId} already was unbooked", ticketId);
                    return;
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
        public async Task PermanentlyBookTicketForPurchaseAsync(Ticket ticket)
        {
            if (ticket == null)
            {
                _logger.LogError("PermanentlyBookTicketForPurchaseAsync: Ticket is null");
                throw new ArgumentNullException("Ticket not found");
            }

            await _timerService.CancelTimerAsync<IBookingService>(ticket.ID);
            _logger.LogInformation( "PermanentlyBookTicketForPurchaseAsync: Canceled unbooking timer if exists");

            try
            {
                var ticketStartTime = ticket.EventSession.StartSessionDateTime;

                ticket.BookedUntil = ticketStartTime;

                await _unitOfWork.SaveAsync();

                _logger.LogInformation("PermanentlyBookTicketForPurchaseAsync: Ticket booked until {BookedUntil}", ticket.BookedUntil); }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
