# Context Map

## Contexts

- [Todos](./src/Todos/CONTEXT.md) — manages todo items through their full lifecycle
- [Notifications](./src/Notifications/CONTEXT.md) — delivers notifications in response to todo domain events

## Relationships

- **Todos → Notifications**: Todos emits `TodoCreatedEvent` and `TodoCompletedEvent` (via DotNetCore.CAP); Notifications consumes both to send user notifications
- **Todos ↔ Contracts**: Todos raises events defined in the shared `Contracts` project (`TodoCreatedEvent`, `TodoCompletedEvent`, `TodoCompleted`)
- **Notifications ↔ Contracts**: Notifications consumes events from the shared `Contracts` project

## Shared kernel

- `src/Contracts/` — shared event types crossing bounded-context boundaries
- `src/BuildingBlocks/` — shared CQRS abstractions, caching, domain primitives (not a bounded context)
- `src/ServiceDefaults/` — Aspire service configuration shared across all .NET projects
