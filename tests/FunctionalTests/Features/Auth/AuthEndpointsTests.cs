using System.Net;
using FunctionalTests.Seedwork;

namespace FunctionalTests.Features.Auth;

public class AuthEndpointsTestsShould(ApiServiceFixture fixture) : ApiTestBase(fixture)
{
    [Fact]
    public async Task ReturnUnauthorized_WhenGetMe_WithoutSession()
    {
        var response = await Given.Client.GetAsync("/auth/me");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AcceptValidLocalReturnUrl_OnLogin()
    {
        var response = await Given.Client.GetAsync("/auth/login?returnUrl=/todos",
            HttpCompletionOption.ResponseHeadersRead);

        // OIDC challenge redirects to Keycloak — any redirect (302/303/307) is acceptable
        response.IsSuccessStatusCode.ShouldBeFalse();
        ((int)response.StatusCode).ShouldBeInRange(300, 399);

        var location = response.Headers.Location?.ToString() ?? string.Empty;
        location.ShouldContain("redirect_uri");
    }

    [Theory]
    [InlineData("bad")]                    // no leading slash
    [InlineData("//evil.com")]             // protocol-relative bypass
    [InlineData("https://evil.com")]       // absolute URL
    public async Task RejectInvalidReturnUrl_OnLogin(string returnUrl)
    {
        var response = await Given.Client.GetAsync(
            $"/auth/login?returnUrl={Uri.EscapeDataString(returnUrl)}",
            HttpCompletionOption.ResponseHeadersRead);

        // Should still trigger OIDC challenge but redirect_uri must fall back to "/"
        ((int)response.StatusCode).ShouldBeInRange(300, 399);

        var location = response.Headers.Location?.ToString() ?? string.Empty;
        // redirect_uri should be encoded "/" not the supplied evil URL
        location.ShouldNotContain("evil.com");
    }
}
