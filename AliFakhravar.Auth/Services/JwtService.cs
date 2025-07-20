using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AliFakhravar.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AliFakhravar.Auth.Services;

/// <summary>
/// Service for generating JSON Web Tokens (JWT) for authenticated users.
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="settings">The JWT settings loaded from configuration.</param>
    /// <param name="userManager">The ASP.NET Identity user manager.</param>
    public JwtService(IOptions<JwtSettings> settings, UserManager<IdentityUser> userManager)
    {
        _settings = settings.Value;
        _userManager = userManager;
    }

    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A <see cref="Task{String}"/> representing the asynchronous operation, with the JWT as its result.</returns>
    public async Task<string> GenerateTokenAsync(IdentityUser user)
    {
        // Get the roles assigned to the user
        var roles = await _userManager.GetRolesAsync(user);

        // Create standard claims
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(ClaimTypes.Name, user.UserName!)
        };

        // Add user roles as claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Generate the signing key and credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the token
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: creds
        );

        // Return the serialized token string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
