# AliFakhravar.Auth

A reusable authentication and authorization component built with ASP.NET Core Identity and JWT. Ideal for .NET Core projects that need plug-and-play user management, secure token-based authentication, and scalability for microservices or monoliths.

---

## ✅ Features

- 🔐 ASP.NET Core Identity integration
- 🔑 JWT-based login system
- 👥 Role and claims support
- 📦 Clean, reusable NuGet package
- ⚙ Extensible architecture for production apps

---

## 📦 Installation

Install via NuGet:

```bash
dotnet add package AliFakhravar.Auth
```

## Configuration

Add the following section in your `appsettings.json`:

```json
{
  "JwtSettings": 
   {
    "SecretKey": "YourVeryStrongSecretKey1234567890",
    "Issuer": "yourapp",
    "Audience": "yourapp_users",
    "ExpiryMinutes": 60
  }
}
```

- **SecretKey**: A strong secret key (at least 32 characters) used to sign JWT tokens.

- **Issuer**: The token issuer, typically your application or domain name.

- **Audience**: The token audience, usually your app users or clients.

- **ExpiryMinutes**: The number of minutes the JWT token remains valid before it expires. For example, 60 means the token is valid for 1 hour.

---

## Usage

1. Register services in `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddIdentityAndJwtAuth<ApplicationUser, ApplicationDbContext>(builder.Configuration);
```
2. Define JWT settings in appsettings.json (as shown above).

3. Inject and use IJwtService and Identity services in your application. Example login service:

```csharp
public class AuthService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(IJwtService jwtService, UserManager<ApplicationUser> userManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null; // Invalid credentials
        }

        return await _jwtService.GenerateTokenAsync(user);
    }
}
```
4. Define ApplicationUser and ApplicationDbContext:

```csharp
public class ApplicationUser : IdentityUser
{
    // Optional additional properties
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

```

5. Configure your database context:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```"# AliFakhravar.Auth" 
