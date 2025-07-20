namespace AliFakhravar.Auth.Models;

/// <summary>
/// Represents the result of an authentication operation, such as login or token generation.
/// </summary>
public class AuthResult
{
    /// <summary>
    /// Indicates whether the authentication operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The JWT access token returned when authentication is successful.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Optional refresh token for obtaining a new access token without re-authenticating.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// A list of error messages explaining why authentication failed.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// The timestamp indicating when this result was created (UTC).
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a failed <see cref="AuthResult"/> with the specified error messages.
    /// </summary>
    /// <param name="errors">An array of error messages describing the failure.</param>
    /// <returns>An <see cref="AuthResult"/> indicating failure.</returns>
    public static AuthResult Failed(params string[] errors)
    {
        return new AuthResult
        {
            Success = false,
            Errors = errors.ToList()
        };
    }

    /// <summary>
    /// Creates a successful <see cref="AuthResult"/> with the given JWT token and optional refresh token.
    /// </summary>
    /// <param name="token">The generated JWT access token.</param>
    /// <param name="refreshToken">An optional refresh token for renewing the access token.</param>
    /// <returns>An <see cref="AuthResult"/> indicating success.</returns>
    public static AuthResult SuccessResult(string token, string? refreshToken = null)
    {
        return new AuthResult
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Returns a string representation of the authentication result for debugging or logging.
    /// </summary>
    public override string ToString()
    {
        return Success ? $"AuthResult [Success: true, Token: {(string.IsNullOrEmpty(Token) ? "null" : "******")}, RefreshToken: {(string.IsNullOrEmpty(RefreshToken) ? "null" : "******")}, Timestamp: {Timestamp:u}]" : $"AuthResult [Success: false, Errors: {string.Join("; ", Errors ?? new List<string>())}, Timestamp: {Timestamp:u}]";
    }
}
