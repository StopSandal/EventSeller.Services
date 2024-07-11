using EventSeller.DataLayer.ExternalDTO;
using EventSeller.DataLayer.ExternalDTO.PaymentSystem;
using EventSeller.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

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

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("ProcessPaymentAsync: Response {response}", responseContent);
                var result = JsonConvert.DeserializeObject<JSONRootObjectResponse<ProcessPaymentResponse>>(responseContent);

                _logger.LogInformation("ProcessPaymentAsync: Payment processed successfully for CardId {CardId}", cardId);
                return result.ProcessPaymentResponse;
            }
            else
            {
                _logger.LogError("CancelPaymentAsync: External payment resource returned error");
                await HandleErrorResponse(response);
                throw new Exception("Response is corrupted");
            }
        }

        /// <inheritdoc />
        public async Task ConfirmPaymentAsync(long transactionId, string confirmationCode)
        {
            var request = new ConfirmPaymentDTO
            {
                TransactionId = transactionId,
                ConfirmationCode = confirmationCode
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);

            var response = await _httpClient.PostAsync(CONFIRM_PAYMENT_PATH, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("ConfirmPaymentAsync: Payment confirmed for TransactionId {TransactionId}", transactionId);
            }
            else
            {
                _logger.LogError("CancelPaymentAsync: External payment resource returned error");
                await HandleErrorResponse(response);
            }
        }

        /// <inheritdoc />
        public async Task CancelPaymentAsync(long transactionId)
        {
            HttpContent content = null;
            var callPath = $"{CANCEL_PAYMENT_PATH}/{transactionId}";

            _logger.LogInformation("CancelPaymentAsync: Sending payment cancellation request for TransactionId {TransactionId}", transactionId);
            var response = await _httpClient.PostAsync(callPath, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("CancelPaymentAsync: Payment cancelled for TransactionId {TransactionId}", transactionId);
            }
            else
            {
                _logger.LogError("CancelPaymentAsync: External payment resource returned error");
                await HandleErrorResponse(response);
            }
        }
        /// <inheritdoc />
        public async Task ReturnPaymentAsync(long transactionId)
        {
            var callPath = $"{RETURN_PAYMENT_PATH}/{transactionId}";
            HttpContent content = null;

            _logger.LogInformation("ReturnPaymentAsync: Sending payment return request for TransactionId {TransactionId}", transactionId);
            var response = await _httpClient.PostAsync(callPath, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("ReturnPaymentAsync: Payment returned for TransactionId {TransactionId}", transactionId);
            }
            else
            {
                _logger.LogError("CancelPaymentAsync: External payment resource returned error");
                await HandleErrorResponse(response);
            }
        }
        /// <summary>
        /// Deserealize and handle error request from external api
        /// </summary>
        /// <param name="response">Unsuccessful response <see cref="HttpResponseMessage"/> </param>
        /// <returns> Asynchronous task </returns>
        /// <exception cref="Exception"></exception>
        private async Task HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.Content != null)
            {
                var errorResponseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseContent);

                if (errorResponse != null)
                {
                    _logger.LogError("Error response: {Status} - {Message}", errorResponse.Status, errorResponse.Message);
                    throw new Exception(errorResponse.Message);
                }
                else
                {
                    _logger.LogError("Error response content could not be deserialized. Content: {ErrorResponseContent}", errorResponseContent);
                    throw new Exception("Unknown error occurred.");
                }
            }
            else
            {
                _logger.LogError("Error response with no content.");
                throw new Exception("Unknown error occurred.");
            }
        }
    }
}
