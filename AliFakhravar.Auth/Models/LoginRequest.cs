using System.ComponentModel.DataAnnotations;

namespace AliFakhravar.Auth.Models;

/// <summary>
/// Represents the request data for a user login operation.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The email address associated with the user's account.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// The user's account password.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// The IP address from which the login request originated. Optional.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Information about the device making the login request (e.g., browser, OS). Optional.
    /// </summary>
    public string? DeviceInfo { get; set; }
}