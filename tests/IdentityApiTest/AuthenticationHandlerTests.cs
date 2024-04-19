﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using XkliburSolutions.Familia.Domain.Constants;
using XkliburSolutions.Familia.Domain.Entities;
using XkliburSolutions.Familia.Domain.Models;
using XkliburSolutions.Familia.Handlers;
using XkliburSolutions.Familia.Resources;

namespace XkliburSolutions.Familia.Tests;

public class AuthenticationHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IStringLocalizer<ErrorMessages>> _localizerMock;

    public AuthenticationHandlerTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null!, null!, null!, null!);
        _configurationMock = new Mock<IConfiguration>();
        _localizerMock = new Mock<IStringLocalizer<ErrorMessages>>();
    }

    [Fact]
    public async Task AuthenticateAsync_NullModel_ReturnsBadRequest()
    {
        // Arrange
        AuthenticationHandler handler = new(_userManagerMock.Object, _roleManagerMock.Object, _configurationMock.Object, _localizerMock.Object);
        _localizerMock.Setup(x => x["InvalidLoginModel"]).Returns(new LocalizedString("InvalidLoginModel", "InvalidLoginModel"));

        // Act
        BadRequest<string> result = (BadRequest<string>)await handler.AuthenticateAsync(null!);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task AuthenticateAsync_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        Authentication authenticationModel = new("testUsername", "Test1234");
        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

        AuthenticationHandler handler = new(_userManagerMock.Object, _roleManagerMock.Object, _configurationMock.Object, _localizerMock.Object);

        // Act
        UnauthorizedHttpResult result = (UnauthorizedHttpResult)await handler.AuthenticateAsync(authenticationModel);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task AuthenticateAsync_UserFound_ReturnsOk()
    {
        // Arrange
        Authentication authenticationModel = new("testUsername", "Test1234");
        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { UserName = "testUsername", Email = "testUsername@mydomain.com", PhoneNumber = "1234567890" });
        _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync([ApplicationRoles.User]);
        _userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>())).ReturnsAsync([]);
        _roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole());
        _roleManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<IdentityRole>())).ReturnsAsync([]);
        _configurationMock.Setup(x => x["JWT:Secret"]).Returns("myverylongsecretphrasetotestthefeaturewithjwt");
        _configurationMock.Setup(x => x["JWT:ValidIssuer"]).Returns("issuer");
        _configurationMock.Setup(x => x["JWT:ValidAudience"]).Returns("audience");

        AuthenticationHandler handler = new(_userManagerMock.Object, _roleManagerMock.Object, _configurationMock.Object, _localizerMock.Object);

        // Act
        Ok<AuthenticationResponse> result = (Ok<AuthenticationResponse>)await handler.AuthenticateAsync(authenticationModel);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }
}
