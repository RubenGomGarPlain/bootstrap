# Proposal: Keycloak Auth with BFF Pattern

**Change ID:** auth-keycloak-bff  
**Date:** 2026-06-16  
**Status:** revised-after-validate  

## Problem

Auth is absent (`CONTEXT.md` § Auth: absent ✗). The SPA must authenticate users without ever
handling tokens directly. Security-best-practice mandates cookies, not localStorage tokens.

## Decision Summary

| Question | Answer |
|----------|--------|
| Keycloak host (dev) | Aspire-managed container |
| BFF placement | Embedded in existing `src/Api/` project |
| SPA token strategy | Cookie-based session (HttpOnly, SameSite=Strict) — SPA never sees tokens |
| Exposed auth endpoints | `GET /auth/login`, `GET /auth/callback`, `POST /auth/logout`, `GET /auth/me` |
| Auth enforcement | Global `RequireAuthorization()` on all endpoints |

## Architecture

```
SPA (Vue 3)
  │  /auth/* and /api/* via Vite proxy
  ▼
Api (BFF embedded)
  ├── GET  /auth/login     → OIDC challenge → redirect to Keycloak
  ├── GET  /auth/callback  → OIDC middleware handles automatically (no stub endpoint)
  ├── POST /auth/logout    → revoke session + Keycloak end_session
  └── GET  /auth/me        → returns user claims from session cookie (roles mapped from realm_access)
  │
  │  Tokens stored server-side (SaveTokens=true); SPA receives only an HttpOnly session cookie
  ▼
Keycloak (Aspire container in dev)
```

The SPA never receives or stores tokens. All OIDC flows happen server-side in the Api.
