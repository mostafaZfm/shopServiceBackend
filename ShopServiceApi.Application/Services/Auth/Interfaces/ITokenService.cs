using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServiceApi.Domain.Entities.Auth;

namespace ShopServiceApi.Application.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateTokenAsync(
           string userId,
           string? userName,
           IList<string> roles);

        Task GenerateRefreshTokenAsync(
            string userId,
            string token);

        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        Task<bool> RevokeRefreshTokenAsync(string token);
    }
}
