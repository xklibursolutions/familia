using Microsoft.AspNetCore.Identity;

namespace XkliburSolutions.Familia.Domain.Entities;

/// <summary>
/// Represents a user in the application with properties and methods specific to the user's identity.
/// This class extends the IdentityUser class, adding application-specific properties and methods
/// that are necessary for the authentication and authorization processes within the application.
/// </summary>
public class ApplicationUser
    : IdentityUser
{
}
