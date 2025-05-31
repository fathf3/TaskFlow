using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.AuthDTOs;
using TaskFlow.Application.Services;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
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
