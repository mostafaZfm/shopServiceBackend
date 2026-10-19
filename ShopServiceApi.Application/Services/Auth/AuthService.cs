using ShopServiceApi.Application.DTOs.Auth;
using ShopServiceApi.Application.Services.Auth.Interfaces;
using System.Threading.Tasks;

namespace ShopServiceApi.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IIdentityAuthService _identityAuthService;

        public AuthService(IIdentityAuthService identityAuthService)
        {
            _identityAuthService = identityAuthService;
        }

        public Task<LoginResponseDto> LoginAsync(LoginRequestDto model)
        {
            return _identityAuthService.LoginAsync(model);
        }
    }
}