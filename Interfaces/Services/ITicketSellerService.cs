using DataLayer.Model;
using EventSeller.DataLayer.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    /*
     Билет -> id -> Get Process Payment -> get answer
                -> Set booking
    Tranc id + confirm -> Payment confirm
                                         -> Set sold + infinity booking
    Tranc id -> cancel after 3 min
            -> no booking
    return money ??                        ->trancId
                 -> table ticketTransaction
    */
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
