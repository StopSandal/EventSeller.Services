namespace EventSeller.Services.Helpers.Constants
{
    internal static class ConfigurationPathConstants
    {
        internal const string TemporalBookingInMinutes = "Booking:TemporalBookingForPurchaseInMinutes";
        internal const string AccessTokenExpirationDays = "JWT:AccessTokenDaysForExpiration";
        internal const string SecretKey = "JWT:Secret";
        internal const string RefreshTokenValidityDays = "JWT:RefreshTokenValidityInDays";
    }
}
