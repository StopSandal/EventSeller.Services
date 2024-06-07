using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces
{
    public interface IJWTFactory
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Task<ClaimsIdentity?> GetIdentityFromExpiredTokenAsync(string? accessToken);
        int GetRefreshTokenValidityInDays();
    }
}
