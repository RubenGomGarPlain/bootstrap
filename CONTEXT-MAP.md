# Context Map

## Contexts

- [Todos](./src/Todos/CONTEXT.md) — creates and manages task items; the system of record for Todo lifecycle
- [Notifications](./src/Notifications/CONTEXT.md) — reacts to domain events and delivers notifications (email + real-time via SignalR)

## Relationships

- **Todos → Notifications**: Todos emits `TodoCreatedEvent`; Notifications consumes it to send a creation email
- **Todos → Notifications**: Todos emits `TodoCompletedEvent`; Notifications consumes it to push a SignalR broadcast and a completion email
- **Shared types:** `TodoCreatedEvent`, `TodoCompletedEvent`, `TodoCompleted` defined in `src/Contracts/`
