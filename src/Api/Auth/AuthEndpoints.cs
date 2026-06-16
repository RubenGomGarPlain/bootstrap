using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Api.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").AllowAnonymous();

        // Trigger OIDC login — redirect to Keycloak.
        // returnUrl MUST be a local relative path to prevent open-redirect attacks.
        group.MapGet("/login", (HttpContext ctx, string? returnUrl) =>
        {
            // StartsWith('/') alone is insufficient — "//evil.com" also starts with '/'
            // and is treated as a protocol-relative external URL by browsers.
            var redirectUri = (!string.IsNullOrEmpty(returnUrl)
                               && returnUrl.StartsWith('/')
                               && !returnUrl.StartsWith("//")
                               && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
                              ? returnUrl
                              : "/";

            return Results.Challenge(
                new AuthenticationProperties { RedirectUri = redirectUri },
                [OpenIdConnectDefaults.AuthenticationScheme]);
        });

        // /auth/callback is intercepted by the OIDC middleware before routing —
        // do NOT register a stub here; it would cause a conflicting route.

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
        }).RequireAuthorization();

        return app;
    }
}
