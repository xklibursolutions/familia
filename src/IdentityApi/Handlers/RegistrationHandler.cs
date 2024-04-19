using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using XkliburSolutions.Familia.Domain.Constants;
using XkliburSolutions.Familia.Domain.Entities;
using XkliburSolutions.Familia.Domain.Models;
using XkliburSolutions.Familia.Resources;

namespace XkliburSolutions.Familia.Handlers;

/// <summary>
/// Handles user registration processes by interacting with the user and role management systems.
/// </summary>
/// <remarks>
/// Initializes a new instance of the RegistrationHandler class with the specified user and role managers.
/// </remarks>
/// <param name="userManager">Provides the APIs for managing user in a persistence store.</param>
/// <param name="localizer">The IStringLocalizer instance used for localizing strings.</param>
public class RegistrationHandler(
    UserManager<ApplicationUser> userManager,
    IStringLocalizer<ErrorMessages> localizer)
{
    /// <summary>
    /// Registers a new user asynchronously based on the provided registration model.
    /// </summary>
    /// <param name="registrationModel">The registration details submitted by the user.</param>
    /// <param name="roles">A list of role names to assign to the new user.</param>
    /// <returns>An IResult representing the outcome of the registration attempt.</returns>
    public async Task<IResult> RegisterAsync(Registration registrationModel, List<string> roles)
    {
        // Check if a user already exists with the same username.
        ApplicationUser? userExists = await userManager.FindByNameAsync(registrationModel.Username!);
        if (userExists != null)
        {
            // If the user exists, return a 500 Internal Server Error without creating a new user.
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        // Create a new ApplicationUser object with the provided email and username.
        ApplicationUser user = new()
        {
            Email = registrationModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registrationModel.Username
        };

        // Attempt to create the new user with the provided password.
        IdentityResult result = await userManager.CreateAsync(user, registrationModel.Password!);

        if (!result.Succeeded)
        {
            // If the user creation failed, return a 500 Internal Server Error.
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        foreach (string role in roles)
        {
            await userManager.AddToRoleAsync(user, role);
        }

        // If the user is created successfully, return an OK response with a success message.
        return Results.Ok(new MessageResponse(localizer["UserCreatedSuccessfully"].Value, ResponseStatus.SUCCESS));
    }
}
