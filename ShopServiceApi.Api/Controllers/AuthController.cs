using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.DTOs;
using ShopServiceApi.Application.DTOs.Auth;
using ShopServiceApi.Application.Services.Auth.Interfaces;
using ShopServiceApi.Infrastructure.Identity;
using ShopServiceApi.Infrastructure.Identity.Auth;

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
            _token = token;
        }
        /// <summary>
        /// ثبت نام کاربر
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterNewUserDto dto)
        {
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));

            var user = new ApplicationUser
            {
                // FirstName = dto.FirstName,
                //  LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,
                //  Province = dto.Province,
                //   City = dto.City,
                //  PostalCode = dto.PostalCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new { success = true, message = "User registered successfully" });
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
            if (user == null)
                return StatusCode(200, LoginResponseDto.Fail("کاربری با این مشخصات پیدا نشد"));
               

            // چک کردن رمز
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
                return StatusCode(200, LoginResponseDto.Fail("کاربری با این مشخصات پیدا نشد"));

            var roles = await _userManager.GetRolesAsync(user);

            // تولید JWT
            var token = _token.GenerateTokenAsync(user, roles);
            // تولید Refresh Token طولانی
            var refToken = Guid.NewGuid().ToString();
            _token.GenerateRefreshTokenAsync(new RefreshToken()
            {
                Token = refToken,
                UserId = user.Id,
            });

            return StatusCode(200, LoginResponseDto.Success(token, refToken, $"کاربر گرامی: {user.FirstName} خوش آمدید"));
           
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto model)
        {
            var refreshTokenEntity = await _token.GetRefreshToken(model.RefreshToken);

            if (refreshTokenEntity == null || refreshTokenEntity.Expiration < DateTime.UtcNow)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(refreshTokenEntity.UserId);
            var roles = await _userManager.GetRolesAsync(user);

            var newJwt =  _token.GenerateTokenAsync(user, roles);

            return StatusCode(200, new LoginResponseDto()
            {
                IsSuccess = true,
                Token = newJwt,
            });
        }

    }
}
