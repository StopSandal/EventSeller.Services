using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Defines methods for managing the selling of tickets, including pricing, availability, purchasing, and payment processing.
    /// </summary>
    public interface ITicketSellerService
    {
        /// <summary>
        /// Asynchronously retrieves the full price information for a ticket by its ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket.</param>
        /// <returns>A task that represents the asynchronous operation, containing the ticket price information.</returns>
        Task<TicketPriceInfoDTO> GetFullTicketPriceAsync(long ticketId);

        /// <summary>
        /// Asynchronously checks if a ticket is available for purchase by its ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket.</param>
        /// <returns>A task that represents the asynchronous operation, containing a boolean indicating whether the ticket is available for purchase.</returns>
        Task<bool> IsTicketAvailableForPurchaseByIdAsync(long ticketId);


        /// <summary>
        /// Asynchronously processes the purchase of a ticket.
        /// </summary>
        /// <param name="purchaseTicketDTO">The data transfer object containing the purchase details.</param>
        /// <returns>A task that represents the asynchronous operation, containing the payment confirmation details.</returns>
        Task<PaymentConfirmationDTO> ProcessTicketBuyingAsync(PurchaseTicketDTO purchaseTicketDTO);

        /// <summary>
        /// Asynchronously confirms the payment for a ticket.
        /// </summary>
        /// <param name="userName">The name of the user who made the payment.</param>
        /// <param name="paymentConfirmationDTO">The data transfer object containing the payment confirmation details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<TicketTransaction> ConfirmTicketPaymentAsync(string userName, PaymentConfirmationDTO paymentConfirmationDTO);

        /// <summary>
        /// Asynchronously cancels the payment for a ticket by its ID and transaction ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket.</param>
        /// <param name="transactionId">The ID of the transaction to cancel.</param>
        /// <returns>A task that represents the asynchronous operation, containing the payment transaction details.</returns>
        Task CancelTicketPaymentAsync(long ticketId, long transactionId);

        /// <summary>
        /// Asynchronously returns the money for a ticket purchase by its transaction ID.
        /// </summary>
        /// <param name="ticketTransactionId">The transaction ID of the ticket purchase.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReturnMoneyForPurchaseAsync(long ticketTransactionId);
    }
}
