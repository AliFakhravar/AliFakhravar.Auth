namespace AliFakhravar.Auth.Models;

/// <summary>
/// Represents the response returned after a successful login operation.
/// Contains the JWT token and its expiration time.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// The JWT access token issued to the authenticated user.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// The exact UTC date and time when the token expires.
    /// </summary>
    public DateTime Expiry { get; set; }
}