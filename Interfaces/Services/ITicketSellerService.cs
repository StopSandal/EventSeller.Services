using DataLayer.Model;
using EventSeller.DataLayer.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ITicketSellerService
    {
        public Task<TicketPriceInfoDTO> GetFullTicketPriceAsync(long ticketId);
        public Task<bool> IsTicketAvailableForPurchaseByIdAsync(long ticketId);
        public bool IsTicketAvailableForPurchase(Ticket ticket);
        public Task<PaymentConfirmationDTO> ProcessTicketBuyingAsync(PurchaseTicketDTO purchaseTicketDTO);
        public Task ConfirmTicketPaymentAsync(string userName,PaymentConfirmationDTO paymentConfirmationDTO);
        public Task CancelTicketPaymentAsync(long ticketId,long transactionId);
        public Task ReturnMoneyForPurchaseAsync(long ticketTransactionId);
    }
}
