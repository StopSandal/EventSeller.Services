using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.ExternalDTO.PaymentSystem;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    /// <summary>
    /// Service for interacting with an external payment system.
    /// Implementation of the interface <see cref="IExternalPaymentService"/>
    /// </summary>
    public class ExternalPaymentService : IExternalPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalPaymentService> _logger;
        private const string MEDIA_TYPE = "application/json";

        private const string EXTERNAL_API_PATH = "http://localhost:5289/api/payment";
        internal const string PROCESS_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/process";
        internal const string CANCEL_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/cancel";
        internal const string CONFIRM_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/confirm";
        internal const string RETURN_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/return";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalPaymentService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making requests to the external API.</param>
        /// <param name="logger">The logger used for logging actions and errors.</param>
        public ExternalPaymentService(HttpClient httpClient, ILogger<ExternalPaymentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProcessPaymentResponse> ProcessPaymentAsync(long cardId, decimal totalAmount, string currency, decimal unreturnableFee)
        {
            var request = new
            {
                CardId = cardId,
                TotalAmount = totalAmount,
                Currency = currency,
                UnreturnableFee = unreturnableFee
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);

            _logger.LogInformation("ProcessPaymentAsync: Sending payment process request for CardId {CardId}", cardId);
            var response = await _httpClient.PostAsync(PROCESS_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessPaymentResponse>(responseContent);

            _logger.LogInformation("ProcessPaymentAsync: Payment processed successfully for CardId {CardId}", cardId);
            return result;
        }

        /// <inheritdoc />
        public async Task ConfirmPaymentAsync(long transactionId, string confirmationCode)
        {
            var request = new
            {
                TransactionId = transactionId,
                ConfirmationCode = confirmationCode
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);

            _logger.LogInformation("ConfirmPaymentAsync: Sending payment confirmation request for TransactionId {TransactionId}", transactionId);
            var response = await _httpClient.PostAsync(CONFIRM_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
            _logger.LogInformation("ConfirmPaymentAsync: Payment confirmed for TransactionId {TransactionId}", transactionId);
        }

        /// <inheritdoc />
        public async Task CancelPaymentAsync(long transactionId)
        {
            var request = new { TransactionId = transactionId };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);

            _logger.LogInformation("CancelPaymentAsync: Sending payment cancellation request for TransactionId {TransactionId}", transactionId);
            var response = await _httpClient.PostAsync(CANCEL_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
            _logger.LogInformation("CancelPaymentAsync: Payment cancelled for TransactionId {TransactionId}", transactionId);
        }

        /// <inheritdoc />
        public async Task ReturnPaymentAsync(long transactionId)
        {
            var request = new { TransactionId = transactionId };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);

            _logger.LogInformation("ReturnPaymentAsync: Sending payment return request for TransactionId {TransactionId}", transactionId);
            var response = await _httpClient.PostAsync(RETURN_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
            _logger.LogInformation("ReturnPaymentAsync: Payment returned for TransactionId {TransactionId}", transactionId);
        }
    }
}
