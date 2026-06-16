# Architecture Checklist

Extracted from engineering-conventions skill.

## SOLID Principles

- [ ] Single Responsibility: each class has one reason to change
- [ ] Open/Closed: extend behavior without modifying existing code
- [ ] Liskov Substitution: subtypes substitutable for base types
- [ ] Interface Segregation: small, focused interfaces
- [ ] Dependency Inversion: depend on abstractions, not concretions

## Modular Architecture

- [ ] Modular boundaries respected (no cross-module direct references)
- [ ] Vertical slice: feature folders, not layer folders
- [ ] No circular dependencies between modules
- [ ] Shared kernel kept minimal
- [ ] Public API per module clearly defined

## Command/Query Separation

- [ ] Commands return void or Result (never data)
- [ ] Queries are side-effect free
- [ ] Handlers are single-purpose
- [ ] Pipeline behaviors for cross-cutting concerns

## Communication & Integration

- [ ] Domain events for cross-module communication
- [ ] Async messaging for eventual consistency where appropriate
- [ ] Anti-corruption layers at integration boundaries
- [ ] Contracts defined and versioned

## Error Handling

- [ ] Result pattern over exceptions for expected failures
- [ ] Exceptions only for unexpected/exceptional situations
- [ ] Global exception handler for unhandled cases
- [ ] Errors logged with context

## Observability

- [ ] Structured logs with semantic context
- [ ] Metrics for business and technical KPIs
- [ ] Distributed traces across service boundaries
- [ ] Dashboards and alerts defined
