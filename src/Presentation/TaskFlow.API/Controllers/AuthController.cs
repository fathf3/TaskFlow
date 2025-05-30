using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Services;
using TaskFlow.Application.DTOs.AuthDTOs;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
            {
                _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
                return BadRequest(new { message = "Invalid email or password" });
            }

            _logger.LogInformation("Login successful for email: {Email}", loginDto.Email);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Register attempt for email: {Email}", registerDto.Email);

            var result = await _authService.RegisterAsync(registerDto);

            if (result == null)
            {
                _logger.LogWarning("Register failed - user already exists for email: {Email}", registerDto.Email);
                return BadRequest(new { message = "User already exists" });
            }

            _logger.LogInformation("Register successful for email: {Email}", registerDto.Email);
            return Ok(result);
        }
    }
}
