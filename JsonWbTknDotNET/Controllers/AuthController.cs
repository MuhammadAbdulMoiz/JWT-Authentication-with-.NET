using JsonWbTknDotNET.Entities;
using JsonWbTknDotNET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JsonWbTknDotNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();

        [HttpPost]
        [Route("Register")]
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

        [HttpPost]
        [Route("login")]
        public ActionResult<string> login(UserDto request)
        {
            if (string.IsNullOrEmpty(user.PassHash) || string.IsNullOrEmpty(request.Password))
                return BadRequest("BadRequest!");
            if (user.UserName != request.UserName)
                return BadRequest("User NotFound");
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PassHash, request.Password) == PasswordVerificationResult.Failed)
                return BadRequest("Incorrect Password");

            string token = "User has loggedIn";

            return Ok(token);
        }
    }
}
