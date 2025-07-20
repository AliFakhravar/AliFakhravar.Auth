using Microsoft.AspNetCore.Identity;

namespace AliFakhravar.Auth.Services;

/// <summary>
/// Defines a contract for JWT token generation services.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT access token asynchronously for the specified Identity user.
    /// </summary>
    /// <param name="user">The user for whom the token is generated.</param>
    /// <returns>A task that represents the asynchronous operation, containing the generated JWT token as a string.</returns>
    Task<string> GenerateTokenAsync(IdentityUser user);
}
