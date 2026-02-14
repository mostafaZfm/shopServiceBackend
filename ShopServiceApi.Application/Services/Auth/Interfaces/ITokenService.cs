using ShopServiceApi.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateTokenAsync(ApplicationUser user, IList<string> roles);

    }
}
