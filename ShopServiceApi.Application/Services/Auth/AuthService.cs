using Microsoft.AspNetCore.Identity;
using ShopServiceApi.Application.DTOs.Auth;
using ShopServiceApi.Application.Services.Auth.Interfaces;
using ShopServiceApi.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.RateLimiting;

namespace ShopServiceApi.Application.Services.Auth
{

    
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager
            ,ITokenService token)
        {
               _userManager = userManager; 
            _signInManager = signInManager;
            _tokenService = token;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
                return LoginResponseDto.Fail("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, model.Password, false);

            if (!result.Succeeded)
                return LoginResponseDto.Fail("Invalid username or password");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.GenerateTokenAsync(user, roles);

            return LoginResponseDto.Success(token);
        }
    }
}
