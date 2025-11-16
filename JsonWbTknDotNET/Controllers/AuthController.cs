using JsonWbTknDotNET.Entities;
using JsonWbTknDotNET.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace JsonWbTknDotNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration): ControllerBase
    {
        public static User user = new();

        [HttpPost("Register")]
        public ActionResult<User> Register(UserDto request)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Password is required.");
            }
            var hash = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.UserName = request.UserName;
            user.PassHash = hash;
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if (string.IsNullOrEmpty(user.PassHash) || string.IsNullOrEmpty(request.Password))
                return BadRequest("BadRequest!");
            if (user.UserName != request.UserName)
                return BadRequest("User NotFound");
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PassHash, request.Password) == PasswordVerificationResult.Failed)
                return BadRequest("Incorrect Password");

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
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
