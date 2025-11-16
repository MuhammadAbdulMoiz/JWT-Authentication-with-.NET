using JsonWbTknDotNET.Entities;
using JsonWbTknDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using JsonWbTknDotNET.Services;
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
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var tokens = await authService.LoginAsync(request);
            if (tokens is null)
                return BadRequest("Invalid Username or Password");

            return Ok(tokens);
        }

        [HttpPost("refreshTkn")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return BadRequest("Invalid client request");

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthentiactedEndPoint()
        {

            return Ok("You are authenticated");

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminEndPoint()
        {
            return Ok("You are an admin");
        }

    }
}
