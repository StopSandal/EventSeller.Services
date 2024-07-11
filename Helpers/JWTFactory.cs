using EventSeller.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventSeller.Services.Helpers
{
    /// <summary>
    /// Implementation of <see cref="IJWTFactory"/>
    /// </summary>
    public class JWTFactory : IJWTFactory
    {
        private const string AccessTokenExpirationDays = "JWT:AccessTokenDaysForExpiration";
        private const string SecretKey = "JWT:Secret";
        private const string RefreshTokenValidityDays = "JWT:RefreshTokenValidityInDays";

        private readonly IConfiguration _configuration;

        public JWTFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <inheritdoc/>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        /// <inheritdoc/>
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(int.Parse(_configuration[AccessTokenExpirationDays])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(_configuration[SecretKey])
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
        /// <inheritdoc/>
        public async Task<ClaimsIdentity?> GetIdentityFromExpiredTokenAsync(string? accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[SecretKey])),
                ValidateLifetime = false
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var result = await tokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);
                return result.ClaimsIdentity;
            }
            catch (SecurityTokenException ex)
            {
                throw new SecurityTokenException($"Token validation failed: {ex.Message}");
            }
        }
        /// <inheritdoc/>
        public int GetRefreshTokenValidityInDays()
        {
            return int.Parse(_configuration[RefreshTokenValidityDays]);
        }
    }
}
