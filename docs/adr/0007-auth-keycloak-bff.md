# 0007. Auth: Keycloak as OIDC Provider with BFF Pattern

Date: 2026-06-16  
Status: Accepted

## Context

Auth was listed as absent in CONTEXT.md and as an open question in CONSTITUTION.md. The
solution needs a production-grade identity provider and a secure way for the Vue 3 SPA to
authenticate without exposing tokens to the browser (localStorage tokens are XSS-vulnerable;
token-in-memory is fragile across page reloads).

Three options were considered:
1. **Rolling custom JWT auth** — high implementation cost, no SSO story.
2. **Azure AD / Entra ID** — vendor-locked, requires cloud in dev.
3. **Keycloak (OIDC)** — open-source, runs locally in Aspire, supports SSO, battle-tested.

For the SPA-to-API communication pattern:
- **Token-in-browser (BFF-less):** SPA receives and stores tokens → XSS risk.
- **BFF (Backend for Frontend):** server holds tokens, issues an HttpOnly session cookie to
  the SPA → token never leaves the server, eliminates token-theft via XSS.

## Decision

- Use **Keycloak** as the OIDC identity provider.
  - Dev: Aspire-managed container (`CommunityToolkit.Aspire.Hosting.Keycloak`), realm imported
    from `local/keycloak/realms/`.
  - Prod: external Keycloak instance (URL injected via config).

- Embed the **BFF** directly in the existing `src/Api/` project.
  - Cookie authentication: HttpOnly, SameSite=Strict, SecurePolicy=Always.
  - OIDC flow: `authorization_code` + PKCE via `Aspire.Keycloak.Authentication` /
    `Keycloak.AuthServices.AspNetCore`.
  - Tokens are stored server-side (`SaveTokens = true`); the SPA never receives them.

- Four BFF auth endpoints exposed under `/auth/`:
  - `GET  /auth/login`    — triggers OIDC challenge
  - `GET  /auth/callback` — OIDC callback (handled by middleware)
  - `POST /auth/logout`   — revokes session + Keycloak end_session
  - `GET  /auth/me`       — returns user claims from session cookie

- **Global `RequireAuthorization()`** on all API endpoints; individual endpoints opt-out
  with `AllowAnonymous()` where needed (swagger, health, `/auth/*`).

- Vite dev-server proxies both `/api/*` and `/auth/*` to the Api service.

## Consequences

**Positive:**
- SPA never touches tokens → eliminates token theft via XSS.
- HttpOnly cookie is invisible to JavaScript → immune to `document.cookie` scraping.
- Keycloak runs locally in Aspire — no cloud account needed to develop or test auth.
- `authorization_code + PKCE` is the current OIDC best practice for web clients.
- Fail-secure by default: all endpoints require auth unless explicitly exempted.

**Negative / Trade-offs:**
- BFF adds a round-trip for every authenticated request (cookie → session lookup → token forward).
- Session state is stored in the Api process; horizontal scaling requires a distributed session
  store (Redis — already present) or sticky sessions.
- Keycloak container increases Aspire startup time and resource usage in dev.
- Realm configuration must be kept in sync between `local/keycloak/realms/` (dev) and the
  production Keycloak instance.
- CSRF protection must be added for state-mutating endpoints (the cookie scheme is vulnerable
  to CSRF without it); use `SameSite=Strict` as the primary mitigation for now.
