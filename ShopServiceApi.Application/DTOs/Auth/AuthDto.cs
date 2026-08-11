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

    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public string RefreshToken { get; set; }   // Refresh Token طولانی

        public static LoginResponseDto Success(string token, string refreshToken, string message)
        {
            return new LoginResponseDto
            {
                IsSuccess = true,
                Token = token,
                RefreshToken = refreshToken,
                Message = message
            };
        }

        public static LoginResponseDto Fail(string message)
        {
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = message
            };
        }

    }
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; }
    }



}
