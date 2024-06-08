using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces
{
    /// <summary>
    /// Interface defining operations related to JSON Web Token (JWT) generation and management.
    /// </summary>
    public interface IJWTFactory
    {
        /// <summary>
        /// Generates a JWT with the specified claims.
        /// </summary>
        /// <param name="claims">The claims to include in the JWT.</param>
        /// <returns>The generated JWT string.</returns>
        string GenerateToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Generates a random refresh token for extended user authentication sessions.
        /// </summary>
        /// <returns>The generated refresh token.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Retrieves the claims identity from an expired access token asynchronously.
        /// </summary>
        /// <param name="accessToken">The expired access token.</param>
        /// <returns>The claims identity from the expired access token.</returns>
        Task<ClaimsIdentity?> GetIdentityFromExpiredTokenAsync(string? accessToken);

        /// <summary>
        /// Retrieves the validity period (in number of days) for a refresh token.
        /// </summary>
        /// <returns>The validity period (in number of days) for a refresh token.</returns>
        int GetRefreshTokenValidityInDays();
    }
}
