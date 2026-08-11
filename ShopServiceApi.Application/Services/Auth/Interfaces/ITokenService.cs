using ShopServiceApi.Infrastructure.Identity;
using ShopServiceApi.Infrastructure.Identity.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateTokenAsync(ApplicationUser user, IList<string> roles);
        Task GenerateRefreshTokenAsync(RefreshToken refreshTokenModel);
        Task<RefreshToken> GetRefreshToken(string token);

    }
}
