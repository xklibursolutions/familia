namespace XkliburSolutions.Familia.Domain.Constants;

/// <summary>
/// Defines the roles available in the application as constants for easy management and access.
/// This static class contains role names as string constants, which are used to assign and
/// check user roles within the application. Having a centralized place for role names helps
/// prevent discrepancies and eases the maintenance of role-related functionality.
/// </summary>
public static class ApplicationRoles
{
    /// <summary>
    /// Role constant for an administrator user, who has access to all the administrative functionalities.
    /// </summary>
    public const string Administrator = "Administrator";

    /// <summary>
    /// Role constant for a standard user, who has access to the basic user functionalities.
    /// </summary>
    public const string User = "User";
}
