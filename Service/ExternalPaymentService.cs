using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.ExternalDTO.PaymentSystem;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public class ExternalPaymentService : IExternalPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IExternalPaymentService> _logger;
        private const string MEDIA_TYPE = "application/json";

        private const string EXTERNAL_API_PATH = "http://localhost:5289/api/payment";
        internal const string PROCESS_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/process";
        internal const string CANCEL_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/cancel";
        internal const string CONFIRM_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/confirm";
        internal const string RETURN_PAYMENT_PATH = $"{EXTERNAL_API_PATH}/return";

        public ExternalPaymentService(HttpClient httpClient, ILogger<ExternalPaymentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProcessPaymentResponse> ProcessPaymentAsync(long cardId, decimal amount, string currency)
        {
            var request = new
            {
                CardId = cardId,
                Amount = amount,
                Currency = currency
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);
            var response = await _httpClient.PostAsync(PROCESS_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessPaymentResponse>(responseContent);

            return result;
        }

        public async Task ConfirmPaymentAsync(long transactionId, string confirmationCode)
        {
            var request = new
            {
                TransactionId = transactionId,
                ConfirmationCode = confirmationCode
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);
            var response = await _httpClient.PostAsync(CONFIRM_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
        }

        public async Task CancelPaymentAsync(long transactionId)
        {
            var request = new { TransactionId = transactionId };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);
            var response = await _httpClient.PostAsync(CANCEL_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
        }

        public async Task ReturnPaymentAsync(long transactionId)
        {
            var request = new { TransactionId = transactionId };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MEDIA_TYPE);
            var response = await _httpClient.PostAsync(RETURN_PAYMENT_PATH, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
