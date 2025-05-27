using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Services;
using TaskFlow.Application.DTOs.AuthDTOs;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
                return BadRequest(new { message = "Invalid email or password" });

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (result == null)
                return BadRequest(new { message = "User already exists" });

            return Ok(result);
        }
    }
}
