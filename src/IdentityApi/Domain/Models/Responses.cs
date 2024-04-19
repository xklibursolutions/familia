namespace XkliburSolutions.Familia.Domain.Models;

/// <summary>
/// Represents a basic response with a status.
/// </summary>
public record Response(string Status);

/// <summary>
/// Represents a response containing data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data.</typeparam>
public record DataResponse<T>(T? Data, string Status) : Response(Status);

/// <summary>
/// Represents a response with an additional message.
/// </summary>
public record MessageResponse(string Message, string Status) : Response(Status);

/// <summary>
/// Represents an authentication response with token information.
/// </summary>
public record AuthenticationResponse(string? Token, DateTime? Expiration, string Status) : Response(Status);
