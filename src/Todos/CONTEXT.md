# Todos

Manages the creation, update, and completion of todo items. This is the primary bounded context
and the sole owner of the `Todo` aggregate.

## Language

**Todo**:
A discrete task or action item with a title and a completion state.
_Avoid_: Task, item, record

**Title**:
The human-readable description of what needs to be done. Must be non-empty.
_Avoid_: Name, label, description

**Complete / CompleteTodo**:
The act of marking a Todo as done. Idempotent — completing an already-completed Todo is a no-op.
_Avoid_: Finish, close, resolve, done

**Completed**:
The boolean state of a Todo after `CompleteTodo` has been called.
_Avoid_: Done, finished, closed

## Domain events

**TodoCreatedEvent**:
Raised when a new Todo is persisted. Carries the new Todo's `Id`.
Published via DotNetCore.CAP to the `Contracts` shared kernel.

**TodoCompletedEvent**:
Raised when `CompleteTodo` transitions a Todo from incomplete to complete.
Carries the Todo's `Id` and `Title`.
Published via DotNetCore.CAP to the `Contracts` shared kernel.
