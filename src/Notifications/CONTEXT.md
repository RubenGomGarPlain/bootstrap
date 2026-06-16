# Notifications

A reactive bounded context. It does not own any domain state — it listens to domain events emitted by Todos and delivers notifications to external channels. Notifications are fire-and-forget; there is no retry or inbox management in the current implementation.

## Language

**Notification**:
A message sent to an external channel (email or real-time push) in response to a domain event.
_Avoid_: Alert, message, event

**Consumer**:
A class that subscribes to a specific domain event via CAP and handles the delivery of a Notification.
_Avoid_: Handler, listener, subscriber

**Email notification**:
A Notification delivered via SMTP using FluentEmail in response to `TodoCreatedEvent` or `TodoCompletedEvent`.
_Avoid_: Mail, message, email alert

**Real-time push**:
A Notification delivered via SignalR (`TodoHub`) to all connected clients in response to `TodoCompletedEvent`.
_Avoid_: WebSocket message, hub message, broadcast

## Events consumed

**TodoCreatedEvent** (from Todos):
Triggers an email notification confirming a new Todo was created.

**TodoCompletedEvent** (from Todos):
Triggers both an email notification and a real-time SignalR push to all connected clients.
