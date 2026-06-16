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

Inside `AddApiServices()`:
- Register Cookie authentication (HttpOnly, SameSite=Strict, Secure, 401 on redirect)
- Register Keycloak OIDC via `AddKeycloakOpenIdConnect` (realm: `bootstrap`, client: `bootstrap-bff`)
- Register `services.AddAuthorization()`

---

## T-05 · Create auth endpoints

**File:** `src/Api/Auth/AuthEndpoints.cs`

Implement `MapAuthEndpoints()` extension with:
- `GET /auth/login` → OIDC challenge
- `GET /auth/callback` → stub (handled by OIDC middleware), AllowAnonymous
- `POST /auth/logout` → SignOut both Cookie + OIDC schemes
- `GET /auth/me` → return user claims JSON, RequireAuthorization

Register in `src/Api/Program.cs` with `app.MapAuthEndpoints()`.

---

## T-06 · Enforce global authorization on API endpoints

**File:** `src/Api/Program.cs`

- Add `app.UseAuthentication()` and `app.UseAuthorization()` to the pipeline (before `UseRouting` / after `UseProblemDetails`)
- Add `.RequireAuthorization()` to the `app.MapEndpoints()` call
- Add `.AllowAnonymous()` to swagger, health, and `/auth/*` endpoints

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

## T-10 · Write ADR

**File:** `docs/adr/0007-auth-keycloak-bff.md`

Document the decision: Keycloak as OIDC IdP, BFF embedded in Api, cookie-based session, global RequireAuthorization.

---

## Execution Order

```
T-01 → T-02 → T-03 → T-04 → T-05 → T-06 → T-07 → T-08 → T-09 → T-10
```

Dependencies:
- T-03 depends on T-01 (Aspire package must be in props first)
- T-04 depends on T-01 (Api packages must be in props first)
- T-05 depends on T-04 (auth config must exist)
- T-06 depends on T-05 (endpoints must exist to apply RequireAuthorization)
- T-07 is independent of .NET tasks
- T-08 is independent of .NET tasks
