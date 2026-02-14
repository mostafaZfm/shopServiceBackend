using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ShopServiceApi.Application.DTOs;
using ShopServiceApi.Application.DTOs.Auth;
using ShopServiceApi.Application.Services.Auth.Interfaces;
using ShopServiceApi.Infrastructure.Identity;

namespace ShopServiceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _token;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IConfiguration configuration,
                          SignInManager<ApplicationUser> signInManager,
                          ITokenService token)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        /// <summary>
        /// ثبت نام کاربر
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                Province = dto.Province,
                City = dto.City,
                PostalCode = dto.PostalCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new { message = "User registered successfully" });
        }


        /// <summary>
        /// ورود کاربر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {           
            // گرفتن یوزر از UserManager
            var user = await _userManager.FindByNameAsync(model.Username);
            var roles = await _userManager.GetRolesAsync(user);

            // تولید JWT
            var token = _token.GenerateTokenAsync(user, roles);

            // پاسخ شامل JWT
            return Ok(new { token });
        }

    }
}
