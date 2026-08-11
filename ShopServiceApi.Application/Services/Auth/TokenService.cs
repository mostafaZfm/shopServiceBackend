using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopServiceApi.Application.Services.Auth.Interfaces;
using ShopServiceApi.Infrastructure.Data;
using ShopServiceApi.Infrastructure.Identity;
using ShopServiceApi.Infrastructure.Identity.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopServiceApi.Application.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration
            ,ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task GenerateRefreshTokenAsync(RefreshToken refreshTokenModel)
        {
            var refreshEntity = new RefreshToken
            {
                Token = refreshTokenModel.Token,
                UserId = refreshTokenModel.UserId,
                Expiration = DateTime.UtcNow.AddDays(7), // 7 روز اعتبار
                IsRevoked = false
            };
            _context.RefreshTokens.Add(refreshEntity);
            await _context.SaveChangesAsync();
        }

        public string GenerateTokenAsync(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked);
        }
    }
}
