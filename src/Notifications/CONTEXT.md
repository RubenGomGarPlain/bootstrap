# Notifications

Reacts to domain events emitted by other bounded contexts and delivers notifications
to end users. This context owns no domain entities of its own — it is purely reactive.

## Language

**Consumer**:
A handler that subscribes to a DotNetCore.CAP event and executes a notification side-effect.
_Avoid_: Subscriber, listener, handler (too generic)

**Notification**:
A message delivered to a user as a consequence of a domain event in another context.
_Avoid_: Alert, message, event

## Consumed events

**TodoCreatedEvent** (from Todos via Contracts):
Triggers a notification informing that a new Todo has been created.
Handled by `TodoCreatedEventConsumer`.

**TodoCompletedEvent** (from Todos via Contracts):
Triggers a notification informing that a Todo has been completed.
Handled by `TodoCompletedEventConsumer`.

## Integration

Notifications are delivered via SignalR (real-time push to connected clients).
Infrastructure lives in `src/Notifications/Infrastructure/`.
