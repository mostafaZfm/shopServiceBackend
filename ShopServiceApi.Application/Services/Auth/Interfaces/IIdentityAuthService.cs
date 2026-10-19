using ShopServiceApi.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Auth.Interfaces
{
    public interface IIdentityAuthService
    {
            Task<LoginResponseDto> LoginAsync(LoginRequestDto model);
       
    }
}
