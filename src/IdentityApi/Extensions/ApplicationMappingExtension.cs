using System.Security.Claims;
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
        // Map the "/api/auth" endpoint for user login
        app.MapPost("/api/auth", async ([FromBody] Authentication authenticationModel, AuthenticationHandler authenticationHandler) =>
        {
            return await authenticationHandler.AuthenticateAsync(authenticationModel);
        })
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
        .WithName("Authenticate")
        .WithOpenApi();

        // Map the "/api/register" endpoint for user registration
        app.MapPost("/api/register", async ([FromBody] Registration registrationModel, RegistrationHandler registrationHandler) =>
        {
            return await registrationHandler.RegisterAsync(registrationModel, [ApplicationRoles.User]);
        })
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces<Response>(StatusCodes.Status200OK)
        .WithName("Register")
        .WithOpenApi();

        // Map the "/api/user" endpoint to retrieve user information
        app.MapGet("/api/users/me", async (ClaimsPrincipal user, UsersHandler usersHandler) =>
        {
            return await usersHandler.GetUserAsync(user);
        })
        .RequireAuthorization()
        .WithName("Me")
        .WithOpenApi();

        return app;
    }
}
