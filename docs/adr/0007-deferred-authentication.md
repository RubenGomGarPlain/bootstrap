# 7. Deferred Authentication Strategy

Date: 2026-06-16
Status: Accepted

## Context

During the constitution ceremony, the discipline-review identified that no authentication
strategy is currently defined. All API endpoints are unauthenticated. This is intentional
for the bootstrap/demo phase of the project: the primary goal is demonstrating the
architectural patterns (Modular Monolith, VSA+CQS, DDD, Aspire) rather than delivering
production-ready security.

Adding an auth layer (e.g., Entra ID BFF, Keycloak, or ASP.NET Core Identity) before the
core patterns are stable would add complexity that obscures the teaching intent.

## Decision

Authentication is explicitly deferred to a future increment. The absence of auth in v1
is a known, conscious trade-off — not an oversight. When auth is introduced, a new ADR
must be written covering: provider choice, BFF vs token-based, and the module that owns
the identity boundary.

## Consequences

✅ Core architectural patterns remain clear and unobscured for demos and teaching
✅ Deferred decision is explicit and traceable
❌ All endpoints are unauthenticated in the current codebase — not suitable for production
❌ Auth retrofit will require changes to `Api`, `BuildingBlocks`, and potentially all modules
