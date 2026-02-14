using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Auth
{
    public sealed class LoginRequestDto
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public string? TwoFactorCode { get; init; }
        public string? TwoFactorRecoveryCode { get; init; }
    }


}
