# Notifications

The notification delivery bounded context. Reacts to domain events emitted by other modules and delivers notifications to users via email and real-time push.

## Language

**Consumer**:
A handler that subscribes to an integration event from the message bus and executes a delivery action.
_Avoid_: Listener, subscriber, handler (too generic)

**TodoCreatedEventConsumer**:
The consumer that reacts to `TodoCreatedEvent` by sending a creation confirmation email.

**TodoCompletedEventConsumer**:
The consumer that reacts to `TodoCompletedEvent` by sending a completion email and broadcasting a real-time update via SignalR.

**TodoHub**:
The SignalR hub through which completion notifications are pushed to all connected clients.
_Avoid_: WebSocket, push channel

**SendCompleted**:
The SignalR hub method that broadcasts a `TodoCompleted` payload to all connected clients.

## Integration events (consumed)

These events originate in `src/Contracts/` and are published by the **Todos** context:

**TodoCreatedEvent** — consumed by `TodoCreatedEventConsumer`
**TodoCompletedEvent** — consumed by `TodoCompletedEventConsumer`
