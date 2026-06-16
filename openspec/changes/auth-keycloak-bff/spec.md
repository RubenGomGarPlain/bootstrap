# Spec: Keycloak Auth with BFF Pattern

**Change ID:** auth-keycloak-bff  
**Status:** ready-for-apply  

---

## 1. Goal

Introduce authentication via Keycloak using the BFF (Backend for Frontend) pattern so the
Vue 3 SPA never handles tokens. The SPA calls four new `/auth/*` endpoints on the Api; the
Api handles all OIDC flows and maintains an HttpOnly session cookie.

---

## 2. Packages Required

Add to `Directory.Packages.props` (no `Version=""` on `<PackageReference>`):

| Package | Version | Project |
|---------|---------|---------|
| `Aspire.Keycloak.Authentication` | latest | `src/Api/Api.csproj` |
| `Microsoft.AspNetCore.Authentication.OpenIdConnect` | latest | `src/Api/Api.csproj` |
| `Keycloak.AuthServices.AspNetCore` | latest | `src/Api/Api.csproj` |
| `CommunityToolkit.Aspire.Hosting.Keycloak` | latest | `src/AspireHost/AspireHost.csproj` |

> Use `dotnet add package` per project **without** specifying versions; let
> `Directory.Packages.props` manage them.

---

## 3. Aspire AppHost Changes (`src/AspireHost/Program.cs`)

```csharp
// 1. Add Keycloak resource (dev container)
var keycloak = builder.AddKeycloak("keycloak", port: 8080)
    .WithDataVolume()                    // persist realm config across restarts
    .WithImportRealms("../local/keycloak/realms");  // realm JSON import on startup

// 2. Give Api a reference to Keycloak
var api = builder.AddProject<Projects.Api>(Api)
    .WithReference(sql)
    .WithReference(cache)
    .WithReference(mail)
    .WithReference(keycloak)            // ← new
    .WithExternalHttpEndpoints()
    .WaitFor(sql)
    .WaitFor(keycloak);                 // ← new
```

Add the realm JSON file at `local/keycloak/realms/bootstrap-realm.json` with:
- Realm name: `bootstrap`
- Client ID: `bootstrap-bff`
- Client secret: `bootstrap-bff-secret` (dev only)
- Redirect URIs: `https://localhost:*/auth/callback`
- Post-logout redirect URI: `https://localhost:*/`
- Direct access grants: disabled (authorization_code only)

---

## 4. Api BFF Configuration (`src/Api/`)

### 4a. `ServiceCollectionExtensions.cs` — `AddApiServices()`

Add BFF cookie + OIDC authentication:

```csharp
services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    // Return 401 JSON (not a redirect) for API calls
    options.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
})
.AddKeycloakOpenIdConnect(
    serviceName: "keycloak",            // matches Aspire resource name
    realm: "bootstrap",
    options =>
    {
        options.ClientId = "bootstrap-bff";
        options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"]
                               ?? "bootstrap-bff-secret";
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.CallbackPath = "/auth/callback";
        options.SignedOutCallbackPath = "/auth/signed-out";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });

services.AddAuthorization();
```

### 4b. `Program.cs` — middleware pipeline

```csharp
app.UseAuthentication();   // ← add before UseRouting
app.UseAuthorization();    // ← add after UseAuthentication

// Global auth enforcement — placed after MapEndpoints()
app.MapEndpoints()
   .RequireAuthorization();  // all endpoints require auth by default

// Swagger / health / auth endpoints must be exempt:
// add .AllowAnonymous() on those specific endpoints
```

---

## 5. Auth Endpoints (`src/Api/Auth/`)

Create `src/Api/Auth/AuthEndpoints.cs`:

```csharp
public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").AllowAnonymous();

        // Trigger OIDC login — redirect to Keycloak
        group.MapGet("/login", (HttpContext ctx, string? returnUrl) =>
        {
            var redirectUri = returnUrl ?? "/";
            return Results.Challenge(
                new AuthenticationProperties { RedirectUri = redirectUri },
                [OpenIdConnectDefaults.AuthenticationScheme]);
        });

        // OIDC callback is handled automatically by the OpenIdConnect middleware
        // but we need a stub endpoint so routing resolves /auth/callback
        group.MapGet("/callback", () => Results.Ok())
             .ExcludeFromDescription();

        // Logout — clear local cookie + trigger Keycloak end_session
        group.MapPost("/logout", async (HttpContext ctx) =>
        {
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await ctx.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "/" });
        });

        // User info — reads claims from the session cookie
        group.MapGet("/me", (HttpContext ctx) =>
        {
            if (ctx.User.Identity?.IsAuthenticated != true)
                return Results.Unauthorized();

            var user = new
            {
                sub      = ctx.User.FindFirst("sub")?.Value,
                name     = ctx.User.FindFirst("name")?.Value,
                email    = ctx.User.FindFirst("email")?.Value,
                username = ctx.User.FindFirst("preferred_username")?.Value,
                roles    = ctx.User.FindAll("roles").Select(c => c.Value),
            };
            return Results.Ok(user);
        }).RequireAuthorization();  // /auth/me requires a valid session

        return app;
    }
}
```

Register in `Program.cs`:

```csharp
app.MapAuthEndpoints();   // add before app.MapEndpoints()
```

---

## 6. Vite Proxy Update (`src/spa/vite.config.ts`)

Add `/auth` proxy alongside the existing `/api` proxy, both pointing to the same Api service:

```typescript
server: {
  host: true,
  port: parseInt(process.env.PORT ?? "5173"),
  proxy: {
    '/api': {
      target: process.env.services__api__https__0 || process.env.services__api__http__0,
      changeOrigin: true,
      rewrite: path => path.replace(/^\/api/, ''),
      secure: false
    },
    '/auth': {                                                     // ← new
      target: process.env.services__api__https__0 || process.env.services__api__http__0,
      changeOrigin: true,
      secure: false
      // no rewrite — /auth/* is forwarded as-is to the BFF
    }
  }
}
```

---

## 7. SPA Auth Composable (`src/spa/src/composables/useAuth.ts`)

```typescript
export interface UserInfo {
  sub: string
  name: string
  email: string
  username: string
  roles: string[]
}

export function useAuth() {
  const user = ref<UserInfo | null>(null)
  const isAuthenticated = computed(() => user.value !== null)

  async function fetchUser() {
    try {
      const res = await fetch('/auth/me')
      if (res.ok) user.value = await res.json()
      else user.value = null
    } catch {
      user.value = null
    }
  }

  function login(returnUrl = '/') {
    window.location.href = `/auth/login?returnUrl=${encodeURIComponent(returnUrl)}`
  }

  async function logout() {
    await fetch('/auth/logout', { method: 'POST' })
    user.value = null
    window.location.href = '/'
  }

  return { user, isAuthenticated, fetchUser, login, logout }
}
```

---

## 8. appsettings.Development.json

The Keycloak authority URL is injected by Aspire via service discovery (`keycloak`). No manual
URLs needed. Add client secret placeholder:

```json
{
  "Keycloak": {
    "ClientSecret": "bootstrap-bff-secret"
  }
}
```

---

## 9. Acceptance Criteria

| # | Criterion |
|---|-----------|
| AC-1 | `GET /auth/login` redirects to Keycloak login page |
| AC-2 | After login, `GET /auth/me` returns `{ sub, name, email, username, roles }` |
| AC-3 | `POST /auth/logout` clears the session cookie and ends the Keycloak session |
| AC-4 | Unauthenticated calls to any `/api/*` endpoint return `401` (not a redirect) |
| AC-5 | SPA `useAuth` composable resolves `isAuthenticated` from `/auth/me` on mount |
| AC-6 | Keycloak starts as a container in Aspire and is reachable at the configured realm URL |
| AC-7 | Vite proxy forwards `/auth/*` and `/api/*` to the Api with no CORS errors |
| AC-8 | Session cookie is HttpOnly, SameSite=Strict, Secure |
