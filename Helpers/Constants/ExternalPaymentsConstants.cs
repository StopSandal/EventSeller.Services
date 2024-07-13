namespace EventSeller.Services.Helpers.Constants
{
    internal class ExternalPaymentsConstants
    {
        internal const string MediaType = "application/json";

        internal const string ExternalApiPath = "http://localhost:5289/api/payment";
        internal const string ProcessPaymentPath = $"{ExternalApiPath}/process";
        internal const string CancelPaymentPath = $"{ExternalApiPath}/cancel";
        internal const string ConfirmPaymentPath = $"{ExternalApiPath}/confirm";
        internal const string ReturnPaymentPath = $"{ExternalApiPath}/return";
    }
}
