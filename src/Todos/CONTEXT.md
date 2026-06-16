# Todos

The core bounded context. Owns the full lifecycle of a task item — creation, update, completion, and deletion. This is the system of record for all Todo state.

## Language

**Todo**:
A discrete unit of work that a user wants to track and complete.
_Avoid_: Task, item, entry

**Title**:
The human-readable description of what needs to be done. Required; may not be blank.
_Avoid_: Name, label, text, description

**Completed**:
The terminal state of a Todo once it has been finished. A completed Todo cannot be completed again.
_Avoid_: Done, finished, closed, resolved

## Commands

**CreateTodo**:
The intention to add a new Todo to the system. Raises `TodoCreatedEvent` on success.
_Avoid_: AddTodo, InsertTodo

**CompleteTodo**:
The intention to mark an existing Todo as Completed. Raises `TodoCompletedEvent` on success.
_Avoid_: FinishTodo, CloseTodo, ResolveTodo

**UpdateTodo**:
The intention to change the Title of an existing Todo.
_Avoid_: EditTodo, ModifyTodo, PatchTodo

**DeleteTodo**:
The intention to permanently remove a Todo from the system.
_Avoid_: RemoveTodo, ArchiveTodo

## Domain Events

**TodoCreatedEvent**:
Raised when a new Todo is successfully created. Carries `TodoId`.

**TodoCompletedEvent**:
Raised when a Todo transitions to the Completed state. Carries `TodoId` and `Title`.
