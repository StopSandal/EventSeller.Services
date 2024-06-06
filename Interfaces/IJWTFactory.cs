using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces
{
    public interface IJWTFactory
    {
        string GenerateToken(IdentityUser user);
    }
}
