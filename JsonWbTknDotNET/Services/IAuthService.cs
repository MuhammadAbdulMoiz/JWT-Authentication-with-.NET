using JsonWbTknDotNET.Models;
using JsonWbTknDotNET.Entities;

namespace JsonWbTknDotNET.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);

        Task<TokenResponseDto?> LoginAsync(UserDto request);

        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
