using Intcomex.ProductsApi.API.Controllers;
using Intcomex.ProductsApi.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AuthControllerTests
{
    [Fact]
    public void Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "bdsgydsgchjbvaucbdosTesting12345!"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var mockLogger = new Mock<ILogger<AuthController>>();
        var controller = new AuthController(configuration, mockLogger.Object);

        var loginDto = new LoginDto
        {
            Username = "admin",
            Password = "123"
        };

        // Act
        var result = controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenProperty = okResult.Value?.GetType().GetProperty("token")?.GetValue(okResult.Value, null)?.ToString();

        Assert.False(string.IsNullOrEmpty(tokenProperty));
    }
}

