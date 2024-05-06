using System.Security.Claims;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;
using XkliburSolutions.Familia.Domain.Constants;
using XkliburSolutions.Familia.Domain.Models;
using XkliburSolutions.Familia.Handlers;

namespace XkliburSolutions.Familia.Extensions;

/// <summary>
/// Extension class for configuring API endpoint mappings.
/// </summary>
public static class ApplicationMappingExtension
{
    /// <summary>
    /// Configures the API endpoint mappings.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    public static WebApplication ConfigureMapping(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder apiVersionGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet)
            .WithOpenApi();

        // Map the "/auth" endpoint for user login
        apiVersionGroup.MapPost("/auth", async ([FromBody] Authentication authenticationModel, AuthenticationHandler authenticationHandler) =>
        {
            return await authenticationHandler.AuthenticateAsync(authenticationModel);
        })
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
        .WithName("Authenticate");

        // Map the "/register" endpoint for user registration
        apiVersionGroup.MapPost("/register", async ([FromBody] Registration registrationModel, RegistrationHandler registrationHandler) =>
        {
            return await registrationHandler.RegisterAsync(registrationModel, [ApplicationRoles.User]);
        })
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces<Response>(StatusCodes.Status200OK)
        .WithName("Register");

        // Map the "/user" endpoint to retrieve user information
        apiVersionGroup.MapGet("/users/me", async (ClaimsPrincipal user, UsersHandler usersHandler) =>
        {
            return await usersHandler.GetUserAsync(user);
        })
        .MapToApiVersion(1, 0)
        .RequireAuthorization()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<DataResponse<User>>(StatusCodes.Status200OK)
        .WithName("Me");

        return app;
    }
}
