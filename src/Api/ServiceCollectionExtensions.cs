using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Api;

internal static class ServiceCollectionExtensions
{
    public static void AddIntegrationCommunucation(this WebApplicationBuilder builder)
    {
        var sql = builder.Configuration.GetConnectionString("TodoAppDb");

        builder.Services.AddCap(options =>
        {
            options.UseInMemoryMessageQueue();

            options.UsePostgreSql(sql!);

            options.UseDashboard();
        });
    }

    public static WebApplicationBuilder AddCustomSeqEndpoint(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetConnectionString("seq") != null)
        {
            builder.AddSeqEndpoint("seq");
        }

        return builder;
    }

    public static WebApplicationBuilder AddBffAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
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
                options.Events.OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            })
            .AddKeycloakOpenIdConnect(
                serviceName: "keycloak",
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
                    options.ClaimActions.MapJsonSubKey("roles", "realm_access", "roles");
                });

        builder.Services.AddAuthorization();
        return builder;
    }
}
