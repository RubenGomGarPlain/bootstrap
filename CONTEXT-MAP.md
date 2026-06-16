# Context Map

## Contexts

- [Todos](./src/Todos/CONTEXT.md) — owns the `Todo` aggregate; manages task lifecycle (create, update, complete)
- [Notifications](./src/Notifications/CONTEXT.md) — reacts to domain events; delivers email and real-time push notifications

## Relationships

- **Todos → Notifications**: Todos emits `TodoCreatedEvent` and `TodoCompletedEvent`; Notifications consumes both via DotNetCore.CAP (RabbitMQ transport)
- **Todos ↔ Contracts**: shared integration events `TodoCreatedEvent`, `TodoCompletedEvent`, `TodoCompleted` defined in `src/Contracts/`
- **Notifications ↔ Contracts**: Notifications reads `TodoCreatedEvent`, `TodoCompletedEvent`, `TodoCompleted` from `src/Contracts/`

## Shared kernel

`src/Contracts/` holds integration event contracts shared across modules. It is not a bounded context — it is an anti-corruption boundary. No business logic belongs here.
