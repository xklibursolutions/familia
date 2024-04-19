using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using XkliburSolutions.Familia.Domain.Constants;
using XkliburSolutions.Familia.Domain.Entities;
using XkliburSolutions.Familia.Domain.Models;

namespace XkliburSolutions.Familia.Handlers;

/// <summary>
/// Handles user-related operations.
/// </summary>
public class UsersHandler(
    UserManager<ApplicationUser> userManager)
{
    /// <summary>
    /// Retrieves current user information asynchronously.
    /// </summary>
    /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the authenticated user.</param>
    /// <returns>An <see cref="IResult"/> containing user details or an unauthorized result.</returns>
    public async Task<IResult> GetUserAsync(ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            string? userName = user.FindFirstValue(ClaimTypes.Name);
            ApplicationUser? applicationUser = await userManager.FindByNameAsync(userName!);

            if (applicationUser == null)
            {
                return Results.BadRequest();
            }

            // Return user details as an OK result
            User userData = new(applicationUser.Id, userName, applicationUser!.Email, applicationUser!.PhoneNumber);
            return Results.Ok(new DataResponse<User>(userData, ResponseStatus.SUCCESS));
        }

        // Return an unauthorized result if user is not authenticated
        return Results.Unauthorized();
    }
}
