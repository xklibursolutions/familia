using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using XkliburSolutions.Familia.Domain.Entities;
using XkliburSolutions.Familia.Domain.Models;
using XkliburSolutions.Familia.Handlers;
using Microsoft.AspNetCore.Http;
using XkliburSolutions.Familia.Resources;
using XkliburSolutions.Familia.Domain.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;

namespace XkliburSolutions.Familia.Tests;

public class RegistrationHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IStringLocalizer<ErrorMessages>> _localizerMock;
    private readonly IConfiguration _configuration;

    public RegistrationHandlerTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _localizerMock = new Mock<IStringLocalizer<ErrorMessages>>();
        IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddUserSecrets<UsersHandlerTests>()
                .AddEnvironmentVariables();
        _configuration = builder.Build();
    }

    [Fact]
    public async Task RegisterAsync_UserExists_ReturnsProblem()
    {
        // Arrange
        var registrationModel = new Registration(_configuration["UnitTestUserName"], "email@mydomain.com", _configuration["UnitTestPassword"]);
        var roles = new List<string> { ApplicationRoles.User };
        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

        RegistrationHandler handler = new(_userManagerMock.Object, _localizerMock.Object);

        // Act
        ProblemHttpResult result = (ProblemHttpResult)await handler.RegisterAsync(registrationModel, roles);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task RegisterAsync_UserCreationFails_ReturnsProblem()
    {
        // Arrange
        var registrationModel = new Registration (_configuration["UnitTestUserName"], "email@mydomain.com", _configuration["UnitTestPassword"]);
        var roles = new List<string> { ApplicationRoles.User };
        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

        RegistrationHandler handler = new(_userManagerMock.Object, _localizerMock.Object);

        // Act
        ProblemHttpResult result = (ProblemHttpResult)await handler.RegisterAsync(registrationModel, roles);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task RegisterAsync_UserCreationSucceeds_ReturnsOk()
    {
        // Arrange
        var registrationModel = new Registration(_configuration["UnitTestUserName"], "email@mydomain.com", _configuration["UnitTestPassword"]);
        var roles = new List<string> { ApplicationRoles.User };
        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _localizerMock.Setup(x => x["UserCreatedSuccessfully"]).Returns(new LocalizedString("UserCreatedSuccessfully", "User created successfully"));

        RegistrationHandler handler = new(_userManagerMock.Object, _localizerMock.Object);

        // Act
        Ok<MessageResponse> result = (Ok<MessageResponse>)await handler.RegisterAsync(registrationModel, roles);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(ResponseStatus.SUCCESS, result.Value!.Status);
    }
}
