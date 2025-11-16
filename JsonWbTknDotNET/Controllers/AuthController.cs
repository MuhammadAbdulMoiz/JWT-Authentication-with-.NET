using JsonWbTknDotNET.Entities;
using JsonWbTknDotNET.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using JsonWbTknDotNET.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace JsonWbTknDotNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new();

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("User already exists");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await authService.LoginAsync(request);
            if (token is null)
                return BadRequest("Invalid Username or Password");

            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthentiactedEndPoint()
        {

            return Ok("You are authenticated");

        }

    }
}
