using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopServiceApi.Abstractions.Interfaces;
using ShopServiceApi.Infrastructure.Data;
using ShopServiceApi.Infrastructure.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ShopServiceApi.Domain.Entities.Auth;
using ShopServiceApi.Application.Services.Auth.Interfaces;

namespace ShopServiceApi.Infrastructure.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;

        public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration
            , IApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public string GenerateTokenAsync(
           string userId,
           string? userName,
           IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "Jwt:Key is not configured.");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task GenerateRefreshTokenAsync(
            string userId,
            string token)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expiration = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t =>
                    t.Token == token &&
                    !t.IsRevoked &&
                    t.Expiration > DateTime.UtcNow);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var affectedRows = await _context.RefreshTokens
                .Where(t =>
                    t.Token == token &&
                    !t.IsRevoked &&
                    t.Expiration > DateTime.UtcNow)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(t => t.IsRevoked, true));

            return affectedRows == 1;
        }
    }
}
