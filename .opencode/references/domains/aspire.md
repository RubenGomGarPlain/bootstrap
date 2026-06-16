# Plane-0 guide: aspire (local dev environment orchestration)

> Knowledge from `dotnet-aspire-devenv`. Configures .NET Aspire as the local dev
> orchestrator. Engine behaviour stripped.

## Objective
Configure .NET Aspire as the local development orchestrator: detect services, map
dependencies to Aspire resources, generate an AppHost with service discovery and an
observability dashboard for zero-setup onboarding.

## Inputs / detection (non-linear — derived from the repo)
- .NET SDK ≥ 8.0.
- Existing `*.AppHost.csproj`? → regenerate or extend (ask).
- Read `CONTEXT.md` / solution structure for layout.
- Existing `docker-compose.yml`? → potential migration source.
- Detect services and their dependencies (DBs, caches, queues) → map to Aspire resources.

## Decision rationale (inline into tasks.md)
- **Aspire as orchestrator:** service discovery + resource wiring + dashboard without per-service boilerplate.
- **Detect, don't redeclare:** services/deps inferred from the solution so local dev matches reality.
- **docker-compose migration path:** existing compose files become the seed for AppHost resources.
- **Observability built-in:** the dashboard gives traces/logs/metrics out of the box.

## What was dropped (now `at-apply`)
Phase preflight checks → bash; AppHost generation/file writes → engine; "regenerate or
extend?" decision → `confirm` task.
