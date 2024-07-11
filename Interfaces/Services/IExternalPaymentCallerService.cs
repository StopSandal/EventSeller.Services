using EventSeller.DataLayer.ExternalDTO.PaymentSystem;

namespace EventSeller.Services.Interfaces.Services
{
    /// <summary>
    /// Defines methods for processing, confirming, canceling, and returning payments through an external payment system.
    /// </summary>
    public interface IExternalPaymentService
    {
        /// <summary>
        /// Processes a payment using the specified card ID, amount, and currency.
        /// </summary>
        /// <param name="cardId">The ID of the card used for the payment.</param>
        /// <param name="amount">The amount to be charged.</param>
        /// <param name="currency">The currency of the payment.</param>
        /// <param name="unreturnableFee">An optional fee that is non-refundable.</param>
        /// <returns>A task that represents the asynchronous operation, containing the response from the payment processing.</returns>
        Task<ProcessPaymentResponse> ProcessPaymentAsync(long cardId, decimal amount, string currency, decimal unreturnableFee = 0);

        /// <summary>
        /// Confirms a payment using the specified transaction ID and confirmation code.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to confirm.</param>
        /// <param name="confirmationCode">The confirmation code for the transaction.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ConfirmPaymentAsync(long transactionId, string confirmationCode);

        /// <summary>
        /// Cancels a payment using the specified transaction ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to cancel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CancelPaymentAsync(long transactionId);

        /// <summary>
        /// Returns a payment using the specified transaction ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to return.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReturnPaymentAsync(long transactionId);
    }
}
