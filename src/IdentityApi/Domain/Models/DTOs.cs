using System.ComponentModel.DataAnnotations;
using XkliburSolutions.Familia.Resources;

namespace XkliburSolutions.Familia.Domain.Models;

/// <summary>
/// Represents user information with optional properties.
/// </summary>
public record User(string? Id, string? Username, string? Email, string? Phone);

/// <summary>
/// Represents user authentication information for login.
/// </summary>
public record Authentication(
    [Required(
        AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = "InvalidUsername")]
    string? Username, // User's username

    [Required(
        AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = "InvalidPassword")]
    string? Password); // User's password

/// <summary>
/// Represents user registration information for sign-up.
/// </summary>
public record Registration(
    [Required(
        AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = "InvalidUsername")]
    string? Username, // User's desired username

    [EmailAddress]
    [Required(
        AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = "InvalidEmailAddress")]
    string? Email, // User's email address

    [Required(
        AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = "InvalidPassword")]
    string? Password); // User's chosen password

