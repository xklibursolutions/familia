using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Moq;
using XkliburSolutions.Familia.Domain.Entities;
using XkliburSolutions.Familia.Handlers;
using Microsoft.AspNetCore.Http;
using XkliburSolutions.Familia.Domain.Constants;
using XkliburSolutions.Familia.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace XkliburSolutions.Familia.Tests;

public class UsersHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly UsersHandler _usersHandler;

    public UsersHandlerTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _usersHandler = new UsersHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = new();

        // Act
        IResult result = await _usersHandler.GetUserAsync(claimsPrincipal);

        // Assert
        Assert.Equal(Results.Unauthorized(), result);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsUserDetails_WhenUserIsAuthenticated()
    {
        // Arrange
        ClaimsIdentity claimsIdentity = new([new Claim(ClaimTypes.Name, "testuser")], "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
        ApplicationUser applicationUser = new() { Id = "1", UserName = "testuser", Email = "testuser@example.com", PhoneNumber = "1234567890" };
        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);

        // Act
        IResult result = await _usersHandler.GetUserAsync(claimsPrincipal);
        Ok<DataResponse<User>> expected = (Ok<DataResponse<User>>)Results.Ok(new DataResponse<User>(new User(applicationUser.Id, applicationUser.UserName, applicationUser.Email, applicationUser.PhoneNumber), ResponseStatus.SUCCESS));

        // Assert
        Assert.IsType<Ok<DataResponse<User>>>(result);
        var okResult = (Ok<DataResponse<User>>)result;

        Assert.Equal(expected.StatusCode, okResult.StatusCode);
        Assert.Equal(expected.Value!.Data!.Id, okResult.Value!.Data!.Id);
        Assert.Equal(expected.Value!.Data!.Username, okResult.Value!.Data!.Username);
        Assert.Equal(expected.Value!.Data!.Phone, okResult.Value!.Data!.Phone);
        Assert.Equal(expected.Value!.Data!.Email, okResult.Value!.Data!.Email);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsBadRequest_WhenUserNotFound()
    {
        // Arrange
        ClaimsIdentity claimsIdentity = new([new Claim(ClaimTypes.Name, "testuser")], "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

        // Act
        IResult result = await _usersHandler.GetUserAsync(claimsPrincipal);

        // Assert
        Assert.Equal(Results.BadRequest(), result);
    }
}