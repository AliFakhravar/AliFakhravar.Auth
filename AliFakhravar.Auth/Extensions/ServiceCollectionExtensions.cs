using System.Text;
using AliFakhravar.Auth.Models;
using AliFakhravar.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AliFakhravar.Auth.Extensions;

/// <summary>
/// Extension methods for registering Identity and JWT authentication in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures and adds ASP.NET Core Identity and JWT-based authentication services to the DI container.
    /// </summary>
    /// <typeparam name="TUser">The user type used for Identity (must inherit from <see cref="IdentityUser"/>).</typeparam>
    /// <typeparam name="TContext">The Entity Framework Core DbContext used for storing Identity data (must inherit from <see cref="DbContext"/>).</typeparam>
    /// <param name="services">The IServiceCollection to which the services will be added.</param>
    /// <param name="configuration">The application configuration, used to bind JWT settings from "JwtSettings" section.</param>
    /// <returns>The original <see cref="IServiceCollection"/> with Identity and JWT services added.</returns>
    /// <remarks>
    /// This method:
    /// - Configures Identity with custom password, lockout, and email requirements.
    /// - Binds JWT settings from configuration.
    /// - Adds JWT token validation with symmetric security key.
    /// - Registers a custom IJwtService for token generation.
    /// </remarks>
    public static IServiceCollection AddIdentityAndJwtAuth<TUser, TContext>(this IServiceCollection services,
        IConfiguration configuration) where TUser : IdentityUser where TContext : DbContext
    {
        // Bind JWT settings from appsettings.json: "JwtSettings" section
        var jwtSettingsSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSettingsSection);

        // Register DbContext and Identity services
        services.AddDbContext<TContext>();

        services.AddIdentity<TUser, IdentityRole>(options =>
        {
            // Configure Identity options (can be customized)
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<TContext>()
        .AddDefaultTokenProviders();

        // Register custom JWT service
        services.AddScoped<IJwtService, JwtService>();

        // Configure JWT Authentication
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
        var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No leeway in token expiration
            };
        });

        return services;
    }
}
