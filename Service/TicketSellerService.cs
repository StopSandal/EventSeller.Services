using AutoMapper;
using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto;
using EventSeller.Services.Interfaces;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class TicketSellerService : ITicketSellerService
    {
        private readonly ITicketService _ticketService;
        private readonly IBookingService _bookingService;
        private readonly IExternalPaymentService _externalPaymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TicketSellerService(ITicketService ticketService, IUnitOfWork unitOfWork, IBookingService bookingService, IExternalPaymentService externalPaymentService, IMapper mapper, IUserService userService)
        {
            _ticketService = ticketService;
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
            _externalPaymentService = externalPaymentService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task CancelTicketPaymentAsync(long ticketId,long transactionId)
        {
            await _bookingService.UnbookTicketByIdAsync(ticketId);
            await _externalPaymentService.CancelPaymentAsync(transactionId);
        }

        public async Task ConfirmTicketPaymentAsync(string userName,PaymentConfirmationDTO paymentConfirmationDTO)
        {
            var ticketId = paymentConfirmationDTO.TicketId;
            var transactionId = paymentConfirmationDTO.TransactionId;
            var confirmationCode = paymentConfirmationDTO.ConfirmationCode;
            try
            {
                await _externalPaymentService.ConfirmPaymentAsync(transactionId, confirmationCode);
            }
            catch
            {
                throw;
            }

            var ticket = await _ticketService.GetByIDAsync(ticketId);

            ticket.isSold = true;
            await _unitOfWork.SaveAsync();

            var paymentInfo = GetFullTicketPrice(ticket);
            var user = await _userService.GetUserByUserNameAsync(userName);
            var newTicketTransaction = new TicketTransaction()
            {
                TicketID = ticketId,
                Date = DateTime.UtcNow,
                TransactionId = transactionId,
                TransactionAmount = paymentInfo.TotalAmount,
                CurrencyType = paymentInfo.CurrencyType,
                userId = user.Id
            };
            await _unitOfWork.TicketTransactionRepository.InsertAsync(newTicketTransaction);
            await _unitOfWork.SaveAsync();

        }

        public async Task<TicketPriceInfoDTO> GetFullTicketPriceAsync(long ticketId)
        {
            var ticket = await _ticketService.GetByIDAsync(ticketId);
            return GetFullTicketPrice(ticket);
        }
        private TicketPriceInfoDTO GetFullTicketPrice(Ticket ticket)
        {
            var bookingFeePercentage = ticket.Event.EventType.BookingFeePercentage;
            return new TicketPriceInfoDTO
            {
                BookingFeePercentage = bookingFeePercentage,
                CurrencyType = ticket.CurrencyType,
                TicketPrice = ticket.Price,
                TotalAmount = ticket.Price * (1 + bookingFeePercentage / 100)
            };
        }
        public async Task<bool> IsTicketAvailableForPurchaseByIdAsync(long ticketId)
        {
            var ticket = await _ticketService.GetByIDAsync(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException("Ticket doesn't exists");
            }
            if (ticket.isSold)
                return false;
            if (_bookingService.IsTicketBooked(ticket))
                return false;
            return true;
        }
        public bool IsTicketAvailableForPurchase(Ticket ticket)
        {
            if(ticket == null)
            {
                throw new InvalidOperationException("Ticket doesn't exists");
            }
            return IsTicketAvailableForPurchase(ticket);
        }

        public async Task<PaymentConfirmationDTO> ProcessTicketBuyingAsync(PurchaseTicketDTO purchaseTicketDTO)
        {
            var ticketId = purchaseTicketDTO.ticketId;
            var cardId = purchaseTicketDTO.cardId;
            var ticket = await _ticketService.GetByIDAsync(ticketId);
            if (!IsTicketAvailableForPurchase(ticket))
                throw new InvalidOperationException("Ticket is not available for purchase. Try later.");
            _bookingService.TemporaryBookTicketForPurchase(ticket);

            var paymentInfo = GetFullTicketPrice(ticket);
            var response = await _externalPaymentService.ProcessPaymentAsync(cardId, paymentInfo.TotalAmount, paymentInfo.CurrencyType);
            
            var result = _mapper.Map<PaymentConfirmationDTO>(response);
            result.TicketId = ticketId;
            return result;
        }

        public async Task ReturnMoneyForPurchaseAsync(long ticketTransactionId)
        {
            var ticketTransaction = await _unitOfWork.TicketTransactionRepository.GetByIDAsync(ticketTransactionId);
            var ticket = await _ticketService.GetByIDAsync(ticketTransaction.TicketID);
            ticket.isSold = false;
            await _unitOfWork.SaveAsync();
        }
    }
}
