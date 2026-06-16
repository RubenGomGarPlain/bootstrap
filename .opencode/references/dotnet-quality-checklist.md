# .NET Quality Checklist

Extracted from dotnet-guardrails skill.

## Build & Project Configuration

- [ ] Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- [ ] Central package management (Directory.Packages.props)
- [ ] Directory.Build.props with shared properties (TreatWarningsAsErrors, ImplicitUsings)
- [ ] .editorconfig with C# naming conventions
- [ ] Architecture tests (NetArchTest or ArchUnitNET)

## Async Patterns

- [ ] No `async void` except event handlers
- [ ] ConfigureAwait(false) in library code
- [ ] Async all the way (no .Result or .Wait())
- [ ] CancellationToken propagated through async chains

## Logging & Observability

- [ ] ILogger<T> structured logging (no string interpolation in log calls)
- [ ] Log levels used correctly (Information, Warning, Error)
- [ ] Health checks registered (/health, /alive)
- [ ] Correlation IDs propagated

## Entity Framework

- [ ] No lazy loading enabled
- [ ] Explicit includes for related data
- [ ] Migrations in separate project
- [ ] DbContext lifetime: scoped (default)
- [ ] No tracking for read-only queries (AsNoTracking)

## API Design

- [ ] No business logic in controllers/endpoints
- [ ] Minimal API preferred over controllers
- [ ] Consistent error response format (ProblemDetails)
- [ ] API versioning strategy defined
- [ ] Request validation at boundary (FluentValidation or DataAnnotations)

## Code Quality

- [ ] No warnings in CI build (TreatWarningsAsErrors)
- [ ] Analyzers enabled (Microsoft.CodeAnalysis.NetAnalyzers)
- [ ] No unused usings or dead code
- [ ] Consistent formatting enforced by .editorconfig
