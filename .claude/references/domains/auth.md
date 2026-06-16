# Plane-0 guide: auth (OIDC BFF authentication)

> Knowledge from `dotnet-auth`. Engine behaviour stripped.

## Objective
Add OIDC Backend-for-Frontend authentication to `*.Web`: an HTTP-only session cookie in the
browser, tokens kept server-side, and `/bff/login`, `/bff/logout`, `/bff/user` endpoints.

## Choice point (non-linear) — identity provider
| Provider | Notes |
|----------|-------|
| `entra-id` | Microsoft Entra ID / Azure AD |
| `keycloak` | self-hosted OIDC |

## Inputs (with defaults)
- provider — `entra-id` or `keycloak` (required).
- cookie name — default `__Host-{Project}.Auth` (must satisfy `__Host-` rules: no `Domain`, `Secure`, `Path=/`).
- `__PROJECT_NAME__` inferred from `.slnx`.

## Pre-flight facts
- Shell present (`.slnx` + `src/{Project}.Web/`); no prior auth (`src/{Project}.Web/Auth/` absent).

## Decision rationale (inline into tasks.md)
- **BFF pattern:** browser never sees tokens — only a hardened session cookie; eliminates token-in-JS exfiltration.
- **`__Host-` cookie prefix:** forces `Secure`, `Path=/`, no `Domain` → strongest cookie scoping.
- **Server-side token store:** refresh/rotation handled server-side; SPA stays thin.
- **Provider abstraction:** same endpoints for Entra ID and Keycloak; only configuration differs.

## What was dropped (now `at-apply`)
Pre-flight checks → bash; "Confirm Before Proceeding" → `confirm`; file writes/Steps → engine.
