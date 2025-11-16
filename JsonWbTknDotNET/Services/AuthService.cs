using JsonWbTknDotNET.Data;
using JsonWbTknDotNET.Entities;
using JsonWbTknDotNET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JsonWbTknDotNET.Services
{
    public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.UserName == request.UserName))
                return null;

            var user = new User();

            if (string.IsNullOrEmpty(request.Password))
                return null;

            var hash = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.UserName = request.UserName;
            user.PassHash = hash;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == request.UserName);

            if (user is null)
                return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PassHash, request.Password) == PasswordVerificationResult.Failed)
                return null;

            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await SaveRefreshTokenAsync(user) 
            };
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await SaveRefreshTokenAsync(user)
            };
        }

        private async Task<User?> ValidateRefreshToken(Guid UserId, string RefreshToken)
        {
            var user =  await context.Users.FindAsync(UserId);
            if (user is null || user.RefreshToken != RefreshToken || user.RefreshTkExpTime <= DateTime.UtcNow)
                return null;

            return user;

        }

        private string CreateRefreshToken(User user)
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private async Task<string> SaveRefreshTokenAsync(User user)
        {
            user.RefreshToken = CreateRefreshToken(user);
            user.RefreshTkExpTime = DateTime.UtcNow.AddDays(7);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user.RefreshToken;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSettings:SecretKey")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new JwtSecurityToken
            (
                issuer: configuration.GetValue<String>("JwtSettings:Issuer"),
                audience: configuration.GetValue<String>("JwtSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);

        }

    }
}
