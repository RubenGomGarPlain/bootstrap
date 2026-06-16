# Todos

The core task-management bounded context. Owns the lifecycle of a `Todo` item from creation through completion.

## Language

**Todo**:
A discrete work item with a title and a completion state. The aggregate root of this context.
_Avoid_: Task, item, work item

**Title**:
The human-readable description of what needs to be done. Required; may not be blank.
_Avoid_: Name, label, description

**Complete** (verb) / **Completed** (adjective):
The act of marking a Todo as done. Idempotent — completing an already-completed Todo has no effect.
_Avoid_: Finish, close, resolve, done (as verb)

**TodoCreatedEvent**:
A domain event raised when a new Todo is successfully created. Carries `TodoId`.
_Avoid_: CreatedNotification, NewTodoEvent

**TodoCompletedEvent**:
A domain event raised when a Todo transitions to the Completed state. Carries `TodoId` and `Title`.
_Avoid_: FinishedEvent, DoneEvent

## Features

**Command**:
A write-intent operation that mutates state (e.g., create Todo, update title, complete Todo).

**Query**:
A read-intent operation that returns data without side effects (e.g., get Todo by id, list Todos).
