using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace XkliburSolutions.IdentityApi.Helpers;

/// <summary>
/// Provides methods for creating and managing JSON Web Tokens (JWTs).
/// </summary>
public class SecurityHelper
{
    /// <summary>
    /// Creates a JWT (JSON Web Token) using the specified claims, secret, issuer, and audience.
    /// </summary>
    /// <param name="authClaims">A list of claims to be encoded in the token.</param>
    /// <param name="secret">A secret key used for token signing.</param>
    /// <param name="validIssuer">The issuer of the token.</param>
    /// <param name="validAudience">The audience of the token.</param>
    /// <returns>A new JwtSecurityToken with the specified claims and expiration set to 3 hours from creation.</returns>
    public static JwtSecurityToken CreateToken(
        IList<Claim> authClaims,
        string secret,
        string validIssuer,
        string validAudience)
    {
        SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(secret));

        JwtSecurityToken token = new(
            issuer: validIssuer,
            audience: validAudience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
