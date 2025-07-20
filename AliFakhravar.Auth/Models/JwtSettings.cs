namespace AliFakhravar.Auth.Models;

/// <summary>
/// Represents the settings required for configuring JWT authentication.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The secret key used to sign the JWT tokens. Must be a strong, secure string.
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// The issuer (typically your application or API) of the JWT token.
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// The intended audience for the JWT token (e.g., your client app).
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// The token expiration time in minutes.
    /// </summary>
    public int ExpiryMinutes { get; set; }
}