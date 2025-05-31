using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskFlow.Application.DTOs.AuthDTOs;
using TaskFlow.Application.Services;
using YourNamespace.Controllers;

namespace TaskFlow.Test.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock; // Arayüzü mock ettik
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>(); // Doğrudan AuthService yerine arayüzü kullan
            _loggerMock = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_authServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ShouldReturnBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "wrong@example.com", Password = "incorrect" };
            _authServiceMock.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync((AuthResponseDto)null);

            // Act
            var result = await _authController.Login(loginDto);

            // Assert


            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = actionResult.Value as dynamic;
            Assert.Equal("Invalid email or password", response.message);




        }
    }

}
