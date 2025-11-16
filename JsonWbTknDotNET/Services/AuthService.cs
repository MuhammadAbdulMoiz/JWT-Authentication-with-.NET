using JsonWbTknDotNET.Data;
using JsonWbTknDotNET.Entities;
using JsonWbTknDotNET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == request.UserName);

            if (user is null)
                return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PassHash, request.Password) == PasswordVerificationResult.Failed)
                return null;

            return CreateToken(user);
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
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
