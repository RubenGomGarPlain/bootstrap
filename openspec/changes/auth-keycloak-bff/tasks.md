# Tasks: Keycloak Auth with BFF Pattern

**Change ID:** auth-keycloak-bff

---

## T-01 · Add packages to Directory.Packages.props and project files

**Files:**
- `Directory.Packages.props` — add `CommunityToolkit.Aspire.Hosting.Keycloak`, `Aspire.Keycloak.Authentication`, `Keycloak.AuthServices.AspNetCore`, `Microsoft.AspNetCore.Authentication.OpenIdConnect`
- `src/AspireHost/AspireHost.csproj` — add `<PackageReference Include="CommunityToolkit.Aspire.Hosting.Keycloak" />`
- `src/Api/Api.csproj` — add `Aspire.Keycloak.Authentication`, `Keycloak.AuthServices.AspNetCore`, `Microsoft.AspNetCore.Authentication.OpenIdConnect`

**Note:** No `Version=""` — central package management.

---

## T-02 · Add Keycloak realm JSON

**File:** `local/keycloak/realms/bootstrap-realm.json`

Create a minimal Keycloak realm export with:
- Realm: `bootstrap`
- Client: `bootstrap-bff` (confidential, authorization_code flow, PKCE)
- Redirect URIs: `https://localhost:*`, `http://localhost:*`
- Post-logout redirect URI: `/*`
- A test user: `testuser` / `Test1234!`

---

## T-03 · Register Keycloak in Aspire AppHost

**File:** `src/AspireHost/Program.cs`

- Add `var keycloak = builder.AddKeycloak("keycloak", port: 8080).WithDataVolume().WithImportRealms("../local/keycloak/realms");`
- Add `.WithReference(keycloak).WaitFor(keycloak)` to the `api` resource

---

## T-04 · Configure BFF authentication in Api

**File:** `src/Api/ServiceCollectionExtensions.cs`

Add a new `AddBffAuthentication(this WebApplicationBuilder builder)` extension method (do **not** modify the existing `AddApiServices()` which does not exist):
- Register Cookie authentication (HttpOnly, SameSite=Strict, Secure, 401 on redirect)
- Register Keycloak OIDC via `AddKeycloakOpenIdConnect` (realm: `bootstrap`, client: `bootstrap-bff`)
- Add `options.ClaimActions.MapJsonSubKey("roles", "realm_access", "roles")` to map Keycloak realm roles to flat `"roles"` claims
- Register `services.AddAuthorization()`
- Call `builder.AddBffAuthentication()` from `Program.cs` after `builder.AddApiServices()`

---

## T-05 · Create auth endpoints

**File:** `src/Api/Auth/AuthEndpoints.cs`

Implement `MapAuthEndpoints()` extension with:
- `GET /auth/login` → OIDC challenge. **Validate `returnUrl`** — only accept local relative paths
  using the full guard:
  `returnUrl.StartsWith('/') && !returnUrl.StartsWith("//") && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative)`
  The `!returnUrl.StartsWith("//")` check is **required** — without it, `//evil.com` passes
  the other two conditions and is treated as a protocol-relative external redirect by browsers.
  Fall back to `"/"` otherwise.
- **Do NOT register a `/auth/callback` stub endpoint.** The OIDC middleware intercepts
  `CallbackPath` before routing; a duplicate endpoint causes routing conflicts.
- `POST /auth/logout` → SignOut both Cookie + OIDC schemes
- `GET /auth/me` → return user claims JSON (`sub`, `name`, `email`, `preferred_username`, `roles`), RequireAuthorization

Register in `src/Api/Program.cs` with `app.MapAuthEndpoints()` (before `app.MapEndpoints()`).

---

## T-06 · Enforce global authorization on API endpoints

**File:** `src/Api/Program.cs`

- Add `UseAuthentication()` and `UseAuthorization()` in the correct order:
  `UseProblemDetails() → UseHttpsRedirection() → UseRouting() → UseAuthentication() → UseAuthorization()`
  **`UseAuthentication` must come AFTER `UseRouting`** (endpoint-aware auth policies require routing to have run first).
- Add `.RequireAuthorization()` to the `app.MapEndpoints()` call
- Add `.AllowAnonymous()` to swagger, health, and `/auth/*` group endpoints

---

## T-07 · Update Vite proxy config

**File:** `src/spa/vite.config.ts`

Add `/auth` proxy entry alongside the existing `/api` entry, pointing to
`process.env.services__api__https__0 || process.env.services__api__http__0` with no path rewrite.

---

## T-08 · Add SPA auth composable

**File:** `src/spa/src/composables/useAuth.ts`

Implement `useAuth()` with `user`, `isAuthenticated`, `fetchUser()`, `login()`, `logout()`.

---

## T-09 · Add appsettings client secret placeholder

**File:** `src/Api/appsettings.Development.json`

Add `"Keycloak": { "ClientSecret": "bootstrap-bff-secret" }`.

---

## T-10 · ~~Write ADR~~ *(already committed)*

`docs/adr/0007-auth-keycloak-bff.md` was committed as part of the planning phase. No action needed.

---

## T-11 · Write auth integration + E2E tests

**Files:**
- `tests/Api.IntegrationTests/Auth/AuthEndpointsTests.cs` — xUnit + `WebApplicationFactory` tests:
  - `GET /auth/me` returns `401` when unauthenticated
  - `GET /auth/login?returnUrl=/todos` — valid local path, **accepted** (RedirectUri = `/todos`)
  - `GET /auth/login?returnUrl=bad` — no leading slash, **rejected** (RedirectUri falls back to `/`)
  - `GET /auth/login?returnUrl=//evil.com` — protocol-relative bypass, **rejected** (RedirectUri falls back to `/`)
  - `GET /auth/login?returnUrl=https://evil.com` — absolute URL, **rejected** (RedirectUri falls back to `/`)
- `tests/E2E/Auth/LoginFlowTests.cs` — Playwright test:
  - Navigate to a protected page → redirected to Keycloak login → log in as `testuser` → redirected back → `/auth/me` returns authenticated user

**Note:** Keycloak integration tests require the Aspire test host (`Aspire.Hosting.Testing`) with the Keycloak container running.

---

## Execution Order

```
T-01 → T-02 → T-03 → T-04 → T-05 → T-06 → T-07 → T-08 → T-09 → T-11
```

Dependencies:
- T-03 depends on T-01 (Aspire package must be in props first)
- T-04 depends on T-01 (Api packages must be in props first)
- T-05 depends on T-04 (auth config must exist)
- T-06 depends on T-05 (endpoints must exist to apply RequireAuthorization)
- T-07 is independent of .NET tasks
- T-08 is independent of .NET tasks
- T-11 depends on T-05 + T-06 (endpoints must be complete before tests are written)
