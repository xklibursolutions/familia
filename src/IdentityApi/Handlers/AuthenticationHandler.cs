using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using XkliburSolutions.Familia.Domain.Constants;
using XkliburSolutions.Familia.Domain.Entities;
using XkliburSolutions.Familia.Domain.Models;
using XkliburSolutions.Familia.Helpers;
using XkliburSolutions.Familia.Resources;

namespace XkliburSolutions.Familia.Handlers;

/// <summary>
/// Handles user authentication requests by interacting with the user and role management systems.
/// This class encapsulates the logic required to authenticate users and manage their roles
/// within the application using the provided configuration settings.
/// </summary>
/// <remarks>
/// The constructor initializes a new instance of the AuthenticationHandler class with the necessary dependencies
/// for user authentication, role management, application configuration, and localization.
/// </remarks>
/// <param name="userManager">The UserManager instance used for user authentication.</param>
/// <param name="roleManager">The RoleManager instance used for managing user roles.</param>
/// <param name="configuration">The configuration settings used by the AuthenticationHandler.</param>
/// <param name="localizer">The IStringLocalizer instance used for localizing strings.</param>
public class AuthenticationHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration,
    IStringLocalizer<ErrorMessages> localizer)
{
    /// <summary>
    /// Handles the user authentication process asynchronously.
    /// </summary>
    /// <param name="authenticationModel">The login details submitted by the user.</param>
    /// <returns>An IResult representing the outcome of the login attempt.</returns>
    public async Task<IResult> AuthenticateAsync(Authentication authenticationModel)
    {
        // Check if the provided login model is null, which indicates bad input.
        if (authenticationModel is null)
        {
            // Return a bad request result with a message indicating the login model is invalid.
            return Results.BadRequest(localizer["InvalidLoginModel"].Value);
        }

        // Attempt to find the user by username.
        ApplicationUser? user = await userManager.FindByNameAsync(authenticationModel.Username!);
        // If the user is found and the password is correct...
        if (user != null && await userManager.CheckPasswordAsync(user, authenticationModel.Password!))
        {
            // Retrieve the roles associated with the user.
            IList<string> userRoles = await userManager.GetRolesAsync(user);

            // Create a list of claims including the user's name and a unique identifier.
            List<Claim> claims =
            [
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.HomePhone, user.PhoneNumber ?? ""),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

            IList<Claim> userClaims = await userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);

            // Add role claims to the list of claims.
            foreach (string userRole in userRoles)
            {
                IdentityRole? role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role!);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            // Create a JWT token using the SecurityHelper class.
            JwtSecurityToken token = SecurityHelper.CreateToken(
                claims,
                configuration["JWT:Secret"]!,
                configuration["JWT:ValidIssuer"]!,
                configuration["JWT:ValidAudience"]!);

            // Return an OK result with the token and its expiration time.
            return Results.Ok(new AuthenticationResponse(
                new JwtSecurityTokenHandler().WriteToken(token),
                token.ValidTo,
                ResponseStatus.SUCCESS));
        }

        // If the user is not found or the password is incorrect, return an unauthorized result.
        return Results.Unauthorized();
    }

}


