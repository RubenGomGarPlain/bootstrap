using E2ETests.Seedwork;

namespace E2ETests.Auth;

[Collection(nameof(CollectionServerFixture))]
public class LoginFlowTests(AppHostFixture app) : PlaywrightTestBase
{
    [Fact]
    public async Task NavigateToProtectedPage_RedirectsToKeycloak_ThenBack_AfterLogin()
    {
        string apiUrl = await app.Web.ResolveUrlAsync("/auth/me");

        // Unauthenticated call returns 401
        var httpClient = await app.Web.CreateHttpClientAsync();
        var meResponse = await httpClient.GetAsync("/auth/me");
        meResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        // Navigate to protected root — SPA should trigger login flow
        string rootUrl = await app.Web.ResolveUrlAsync("/");
        await Page.GotoAsync(rootUrl);

        // Wait for redirect to Keycloak login page
        await Page.WaitForURLAsync(url => url.Contains("/realms/bootstrap/protocol/openid-connect/auth"),
            new() { Timeout = 30_000 });

        // Fill in Keycloak login form
        await Page.FillAsync("#username", "testuser");
        await Page.FillAsync("#password", "Test1234!");
        await Page.ClickAsync("[type=submit]");

        // Wait for redirect back to the app
        await Page.WaitForURLAsync(url => !url.Contains("keycloak") && !url.Contains("openid-connect"),
            new() { Timeout = 30_000 });

        // /auth/me should now return the authenticated user
        var cookieHeader = await GetCookieHeaderAsync();
        var authenticatedClient = await app.Web.CreateHttpClientAsync();
        authenticatedClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);
        var authenticatedResponse = await authenticatedClient.GetAsync("/auth/me");

        authenticatedResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    private async Task<string> GetCookieHeaderAsync()
    {
        var cookies = await Page.Context.CookiesAsync();
        return string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));
    }
}
