using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkWeekPlanner.Api.Features.Login.Models;
using WorkWeekPlanner.Api.Features.Login.Services;
using WorkWeekPlanner.Api.Features.Settings;

namespace WorkWeekPlanner.Api.Tests.Features.Login.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAppSettings> _appSettingsMock;
    private readonly IAuthService _sut;
    private const string TestUser = "testuser";
    private const string TestUserPassword = "password";
    private const string IncorrectPassword = "wrongpassword";
    private const string TestUserId = "3B9D6C39-6D76-4D92-B4D6-14899A3C58C7";

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _appSettingsMock = new Mock<IAppSettings>();

        _appSettingsMock.Setup(a => a.AppConfiguration).Returns(new AppConfiguration
        {
            Key = "TestKey123456789012345678901234567890", // 32 characters for HMACSHA256
            ExpiryInHours = 1
        });

        _sut = new AuthService(_userRepositoryMock.Object, _appSettingsMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((IAppUser?)null);

        // Act
        var actual = await _sut.AuthenticateAsync(TestUser, TestUserPassword);

        // Assert
        actual.IsAuthenticated.Should().BeFalse();
        actual.Token.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new Mock<IAppUser>();
        user.Setup(u => u.Id).Returns(TestUserId);
        user.Setup(u => u.Username).Returns(TestUser);
        user.Setup(u => u.Password).Returns(TestUserPassword);

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user.Object);

        // Act
        var actual = await _sut.AuthenticateAsync(TestUser, IncorrectPassword);

        // Assert
        actual.IsAuthenticated.Should().BeFalse();
        actual.Token.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnTrueAndToken_WhenCredentialsAreCorrect()
    {
        // Arrange
        var user = new Mock<IAppUser>();
        user.Setup(u => u.Id).Returns(TestUserId);
        user.Setup(u => u.Username).Returns(TestUser);
        user.Setup(u => u.Password).Returns(TestUserPassword);
        user.Setup(u => u.Roles).Returns(["Admin", "User"]);

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user.Object);

        // Act
        var actual = await _sut.AuthenticateAsync(TestUser, TestUserPassword);

        // Assert
        actual.IsAuthenticated.Should().BeTrue();
        actual.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, TestUser),
            new(ClaimTypes.Role, "Admin")
        };

        // Act
        var token = _sut.GenerateToken(claims);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettingsMock.Object.AppConfiguration.Key);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var actual);

        actual.Should().NotBeNull();
        actual.Should().BeOfType<JwtSecurityToken>();
    }
}