using AutoMapper;
using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service for managing the sale of tickets.
    /// </summary>
    public class TicketSellerService : ITicketSellerService
    {
        private readonly ITicketService _ticketService;
        private readonly IBookingService _bookingService;
        private readonly IExternalPaymentService _externalPaymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketSellerService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSellerService"/> class.
        /// </summary>
        /// <param name="ticketService">The ticket service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="bookingService">The booking service.</param>
        /// <param name="externalPaymentService">The external payment service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="logger">The logger.</param>
        public TicketSellerService(
            ITicketService ticketService,
            IUnitOfWork unitOfWork,
            IBookingService bookingService,
            IExternalPaymentService externalPaymentService,
            IMapper mapper,
            IUserService userService,
            ILogger<TicketSellerService> logger)
        {
            _ticketService = ticketService;
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
            _externalPaymentService = externalPaymentService;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the ticket does not exist.</exception>
        public async Task CancelTicketPaymentAsync(long ticketId, long transactionId)
        {
            _logger.LogInformation("CancelTicketPaymentAsync: Cancelling ticket payment for TicketId {TicketId} and TransactionId {TransactionId}", ticketId, transactionId);
            if ((await _ticketService.GetByIDAsync(ticketId)) == null)
            {
                _logger.LogWarning("CancelTicketPaymentAsync: Ticket is null");
                throw new InvalidOperationException("Ticket doesn't exist");
            }
            await _bookingService.UnbookTicketByIdAsync(ticketId);
            _logger.LogInformation("CancelTicketPaymentAsync: Ticket unbooked for TicketId {TicketId}", ticketId);

            await _externalPaymentService.CancelPaymentAsync(transactionId);
            _logger.LogInformation("CancelTicketPaymentAsync: Payment cancelled for TransactionId {TransactionId}", transactionId);
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the ticket does not exist or is already sold.</exception>
        /// <exception cref="Exception">Thrown when there is an error confirming the payment.</exception>
        public async Task<TicketTransaction> ConfirmTicketPaymentAsync(string userName, PaymentConfirmationDTO paymentConfirmationDTO)
        {
            var ticketId = paymentConfirmationDTO.TicketId;
            var transactionId = paymentConfirmationDTO.TransactionId;
            var confirmationCode = paymentConfirmationDTO.ConfirmationCode;

            _logger.LogInformation("ConfirmTicketPaymentAsync: Confirming ticket payment for TicketId {TicketId}, TransactionId {TransactionId}, and UserName {UserName}", ticketId, transactionId, userName);
           
            var ticket = await _ticketService.GetByIDAsync(ticketId);
            if (ticket == null)
            {
                _logger.LogWarning("IsTicketAvailableForPurchaseByIdAsync: TicketId {TicketId} does not exist", ticketId);
                throw new InvalidOperationException("Ticket doesn't exist");
            }

            var paymentInfo = await GetFullTicketPriceAsync(ticket.ID);
            var user = await _userService.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                _logger.LogCritical("GetUserByUserNameAsync: User {UserName} does not exist", userName);
                throw new InvalidOperationException("User doesn't exist");
            }

            try
            {
                await _externalPaymentService.ConfirmPaymentAsync(transactionId, confirmationCode);
                _logger.LogInformation("ConfirmTicketPaymentAsync: Payment confirmed for TransactionId {TransactionId}", transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConfirmTicketPaymentAsync: Error confirming payment for TransactionId {TransactionId}", transactionId);
                throw;
            }

            ticket.isSold = true;

            var newTicketTransaction = new TicketTransaction()
            {
                TicketID = ticketId,
                Date = DateTime.UtcNow,
                TransactionId = transactionId,
                TransactionAmount = paymentInfo.TotalAmount,
                UnreturnableFee = paymentInfo.BookingAmount,
                CurrencyType = paymentInfo.CurrencyType,
                userId = user.Id
            };

            await _unitOfWork.TicketTransactionRepository.InsertAsync(newTicketTransaction);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("ConfirmTicketPaymentAsync: Ticket sold and transaction saved for TicketId {TicketId}", ticketId);
            return newTicketTransaction;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the ticket does not exist.</exception>
        public async Task<TicketPriceInfoDTO> GetFullTicketPriceAsync(long ticketId)
        {
            _logger.LogInformation("GetFullTicketPriceAsync: Getting full ticket price for TicketId {TicketId}", ticketId);

            var ticket = await _ticketService.GetTicketWithAllIncudesByIdAsync(ticketId);

            if (ticket == null || ticket.Event == null || ticket.Event.EventType==null)
            {
                _logger.LogWarning("GetFullTicketPriceAsync: Ticket is null or one of it's parents");
                throw new InvalidOperationException("Ticket doesn't exist or corrupted");
            }

            _logger.LogInformation("GetFullTicketPriceAsync: Full ticket price retrieved for TicketId {TicketId}", ticketId);

            var bookingFeePercentage = ticket.Event.EventType.BookingFeePercentage;
            var bookingAmount = ticket.Price * (bookingFeePercentage / 100);

            return new TicketPriceInfoDTO
            {
                BookingFeePercentage = bookingFeePercentage,
                CurrencyType = ticket.CurrencyType,
                TicketPrice = ticket.Price,
                BookingAmount = bookingAmount,
                TotalAmount = ticket.Price + bookingAmount
            };
        }


        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the ticket does not exist.</exception>
        public async Task<bool> IsTicketAvailableForPurchaseByIdAsync(long ticketId)
        {
            _logger.LogInformation("IsTicketAvailableForPurchaseByIdAsync: Checking availability for TicketId {TicketId}", ticketId);

            var ticket = await _ticketService.GetByIDAsync(ticketId);

            if (ticket == null)
            {
                _logger.LogWarning("IsTicketAvailableForPurchase: Ticket is null");
                throw new InvalidOperationException("Ticket doesn't exist");
            }

            var isNotAvailable = ticket.isSold || _bookingService.IsTicketBooked(ticket);

            if (isNotAvailable) 
            {
                _logger.LogInformation("IsTicketAvailableForPurchase: Ticket availability for TicketId {TicketId} is {IsAvailable}", ticket.ID, !isNotAvailable);

                return false;
            }

            var ticketStartTime = await _ticketService.GetTicketEventStartDateTimeByIdAsync(ticketId);

            if (ticketStartTime.HasValue)
            {
                _logger.LogInformation("IsTicketAvailableForPurchase: Ticket {TicketId} StartTime is {ticketStartTime}. Now is {DateTime.UtcNow}", ticket.ID, ticketStartTime.Value, DateTime.UtcNow);

                var isTicketNotExpired = ticketStartTime.Value > DateTime.UtcNow;

                _logger.LogInformation("IsTicketAvailableForPurchase: Ticket availability for TicketId {TicketId} is {IsAvailable}", ticket.ID, isTicketNotExpired);

                return isTicketNotExpired;
            }
            else
            {
                _logger.LogWarning("IsTicketAvailableForPurchase: TicketStartDateTime is null");
            }
            return true;

        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the ticket is not available for purchase.</exception>
        public async Task<PaymentConfirmationDTO> ProcessTicketBuyingAsync(PurchaseTicketDTO purchaseTicketDTO)
        {
            var ticketId = purchaseTicketDTO.ticketId;
            var cardId = purchaseTicketDTO.cardId;

            _logger.LogInformation("ProcessTicketBuyingAsync: Processing ticket buying for TicketId {TicketId}", ticketId);

            var ticket = await _ticketService.GetByIDAsync(ticketId);

            if (ticket == null)
            {
                _logger.LogWarning("ProcessTicketBuyingAsync: Ticket is null");
                throw new InvalidOperationException("Ticket doesn't exist");
            }
            var isTicketAvailable = await IsTicketAvailableForPurchaseByIdAsync(ticketId);

            if (!isTicketAvailable )
            {
                _logger.LogWarning("ProcessTicketBuyingAsync: TicketId {TicketId} is not available for purchase", ticketId);
                throw new InvalidOperationException("Ticket is not available for purchase. Try later.");
            }

            await _bookingService.TemporaryBookTicketForPurchaseAsync(ticket);
            _logger.LogInformation("ProcessTicketBuyingAsync: TicketId {TicketId} booked temporarily", ticketId);

            var paymentInfo = await GetFullTicketPriceAsync(ticket.ID);
            var response = await _externalPaymentService.ProcessPaymentAsync(cardId, paymentInfo.TotalAmount, paymentInfo.CurrencyType, paymentInfo.BookingAmount);

            var result = _mapper.Map<PaymentConfirmationDTO>(response);
            result.TicketId = ticketId;

            _logger.LogInformation("ProcessTicketBuyingAsync: Ticket buying processed successfully for TicketId {TicketId}", ticketId);

            return result;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the ticket transaction does not exist or the ticket is already returned.</exception>
        /// <exception cref="Exception">Thrown when there is an error returning the payment.</exception>
        public async Task ReturnMoneyForPurchaseAsync(long ticketTransactionId)
        {
            _logger.LogInformation("ReturnMoneyForPurchaseAsync: Returning money for TicketTransactionId {TicketTransactionId}", ticketTransactionId);

            var ticketTransaction = await _unitOfWork.TicketTransactionRepository.GetByIDAsync(ticketTransactionId);
            if (ticketTransaction == null)
            {
                _logger.LogWarning("ReturnMoneyForPurchaseAsync: TicketTransactionId {TicketTransactionId} does not exist", ticketTransactionId);
                throw new InvalidOperationException("TicketTransaction doesn't exist");
            }
            if (ticketTransaction.IsReturned)
            {
                _logger.LogWarning("ReturnMoneyForPurchaseAsync: TicketTransactionId {TicketTransactionId} is already returned", ticketTransactionId);
                throw new InvalidOperationException("Ticket is already returned");
            }

            var ticket = await _ticketService.GetByIDAsync(ticketTransaction.TicketID);

            var ticketEventBegins = await _ticketService.GetTicketEventStartDateTimeByIdAsync(ticketTransaction.TicketID);

            if(ticketEventBegins == null)
            {
                _logger.LogWarning("GetFullTicketPriceAsync: ticketEventBegins is null"); // here should be used marked action is ticked was used
            }
            else
            {
                var minutesForReturn = - ticket.Event.EventType.MinutesForMoneyReturn;
                var noReturnTime = ticketEventBegins.Value.AddMinutes(minutesForReturn);
                if (DateTime.UtcNow > noReturnTime)
                {
                    _logger.LogWarning("GetFullTicketPriceAsync: Time for return is gone");
                    throw new InvalidOperationException("Unable to return ticket. Time for return expired.");
                }
            }

            try
            {
                await _externalPaymentService.ReturnPaymentAsync(ticketTransaction.TransactionId);
                _logger.LogInformation("ReturnMoneyForPurchaseAsync: Payment returned for TransactionId {TransactionId}", ticketTransaction.TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReturnMoneyForPurchaseAsync: Error returning payment for TransactionId {TransactionId}", ticketTransaction.TransactionId);
                throw;
            }

            ticket.isSold = false;
            ticketTransaction.IsReturned = true;
            
            await _unitOfWork.SaveAsync();

            await _bookingService.UnbookTicketByIdAsync(ticket.ID);
            _logger.LogInformation("ReturnMoneyForPurchaseAsync: Ticket unbooked and status updated for TicketId {TicketId}", ticketTransaction.TicketID);
        }
    }
}
