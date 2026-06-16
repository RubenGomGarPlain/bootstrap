# Greenfield interview

Interview one question at a time. Give a recommended answer and explain why it matters. If
`CONTEXT.md` already exists, read it first and skip answered questions.

## Phase 1 — Seven questions

1. **What are you building, and for whom?**
   _(No recommendation — establishes the domain context for the rest.)_

2. **Deployment topology** — how are deployables organized?
   Options: `modular-monolith` / `microservices` / `single-service`
   Recommendation: `modular-monolith`
   Why: starts with Aspire from day 1, deploys as a single binary, and scales to
   microservices by extracting modules when there is a real reason.

3. **Internal module organization** — how is code organized inside each module?
   Options: `VSA+CQS` / `Clean Architecture` / `minimal`
   Recommendation: `VSA+CQS`
   Why: Vertical Slice Architecture groups by feature, not layer. CQS separates reads from
   writes. Together they reduce coupling and make each feature navigable independently.

4. **Auth from day 1?**
   Options: `yes` / `no`
   Recommendation: `yes`
   Why: adding authentication after data is in production is costly. The BFF+OIDC pattern is
   an HTTP-only cookie — little code, strong security.

5. **Frontend?** — what kind of frontend do you need?
   Options: `React+Vite` / `Angular` / `Vue` / `Blazor` / `none`
   Recommendation: `React+Vite`
   Why: largest ecosystem and fastest startup. Registered as an npm resource in the Aspire
   AppHost — zero CORS configuration.

6. **Project name** — PascalCase identifier for namespaces and file names.
   _(No recommendation — free text. Example: `Contoso`, `MyApp`, `Acme`.)_

7. **Database provider** — which persistence engine?
   Options: `PostgreSQL` / `SQL Server` / `none (deferred)`
   Recommendation: `PostgreSQL`
   Why: PostgreSQL runs free on any cloud and locally via the Aspire container resource
   (`AddPostgres`). SQL Server requires a production license or the Express edition with a
   10 GB size limit. The provider determines the Aspire resource in AppHost and the EF Core
   NuGet package in `Directory.Packages.props` — switching after migrations exist means
   dropping and regenerating the entire schema. Deferring blocks the AppHost scaffold and
   should be flagged as a blocker.

## Phase 2 — Folder structure

Generate a folder tree adapted to the answers. Show it immediately. Ask: "Should we change
any folder before continuing?"

Rules:
- Always include: AppHost, ServiceDefaults, BuildingBlocks, Web, ArchTests, `docs/adr/`,
  `docs/domain-guides/`, CONTEXT.md, `.slnx`
- Add `src/Modules/<module>/` per business module named in Q1 (infer 1–3)
- Add `src/frontend/` only if Q5 ≠ none
- Add `src/{Project}.Auth/` only if Q4 = yes
- Use `{Project}` as the name from Q6
- If Q7 = `PostgreSQL` → AppHost wires `AddPostgres`; note `Npgsql.EntityFrameworkCore.PostgreSQL` required in `Directory.Packages.props`
- If Q7 = `SQL Server` → AppHost wires `AddSqlServer`; note `Microsoft.EntityFrameworkCore.SqlServer` required in `Directory.Packages.props`

## Phase 3 — Pending decisions debate

Debate each item one at a time with a recommendation. Finish when the user says "done".

1. **Central Package Management?** (`Directory.Packages.props`)
   Options: `yes` / `no`
   Recommendation: `yes`
   Why: single source of version truth across all projects. Without CPM, each module's
   `.csproj` pins its own package versions independently — version drift is invisible until
   a conflict surfaces at runtime. Adding CPM after modules diverge requires diffing and
   merging every `.csproj` by hand.

2. **ArchTests from the start?**
   Options: `yes` / `no`
   Recommendation: `yes`
   Why: architecture rules enforced as failing tests catch violations the moment they are
   introduced. Retrofitting ArchTests onto an established codebase means fixing every
   existing violation before the suite can go green — the larger the codebase, the higher
   that cost.

3. **OpenSpec / spec-driven workflow?**
   Options: `yes` / `no`
   Recommendation: `yes`
   Why: decisions documented before code prevent intent-implementation drift. Without a spec
   engine, design decisions live only in commit messages and chat history. The daily loop
   (`at-plan → at-apply → at-validate`) depends on OpenSpec — skipping it means the team
   loses the structured feedback loop between specification and implementation.

4. **Aspire workload installed locally?**
   Options: `installed` / `not installed`
   Recommendation: `installed`
   Why: prerequisite — AppHost cannot run without the Aspire workload. This is not a
   preference; it is a blocker. Install with: `dotnet workload install aspire`.

5. **Docker available?**
   Options: `available` / `not available`
   Recommendation: `available` (required if topology = microservices)
   Why: Aspire spins containers for infrastructure dependencies (databases, message queues,
   caches). Without Docker those resources must be started and stopped manually each session.
   Required if microservices topology was chosen in Phase 1 Q2.

6. **Guardrails?** (`.editorconfig` + nullable refs + conventions companion)
   Options: `yes` / `no`
   Recommendation: `yes`
   Why: one-time setup that pays across every PR review. `.editorconfig` enforces consistent
   style; nullable reference types (`<Nullable>enable</Nullable>`) surface nullability bugs
   at compile time; the conventions companion documents the rules for new team members.
   Retrofitting after code exists triggers a large formatting commit that pollutes `git blame`
   and every in-flight PR until it merges.

7. **API documentation?**
   Options: `Scalar` / `Swashbuckle` / `none`
   Recommendation: `Scalar`
   Why: Scalar ships natively with .NET 9 minimal API — no extra package, modern UI,
   OpenAPI-compliant. Swashbuckle has no official .NET 9 support and its last release
   predates minimal API patterns. Choosing `none` means developers must curl endpoints or
   guess contracts, slowing down both frontend integration and API testing.

8. **Global exception handling?**
   Options: `ProblemDetails middleware` / `manual try-catch` / `none`
   Recommendation: `ProblemDetails middleware`
   Why: one registration in `Program.cs` gives every endpoint RFC-7807-compliant error
   responses (`type / title / status / detail`). Manual try-catch duplicates error-shaping
   logic across every handler and produces inconsistent shapes over time. `none` leaks stack
   traces to clients and breaks any frontend error handling that parses a known structure.
   Register with: `builder.Services.AddProblemDetails()` + `app.UseExceptionHandler()`.

9. **Global JSON serialization?**
   Options: `explicit System.Text.Json options` / `framework defaults`
   Recommendation: `explicit options`
   Why: framework defaults serialize property names as PascalCase and enums as integers.
   Frontend developers encounter both mismatches on the first GET request. Set once in
   `Program.cs`: `PropertyNamingPolicy = JsonNamingPolicy.CamelCase`,
   `Converters.Add(new JsonStringEnumConverter())`,
   `DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull`.

10. **Domain error strategy?**
    Options: `ErrorOr<T>` / `FluentResults` / `throw exceptions`
    Recommendation: `ErrorOr<T>`
    Why: domain errors (not found, conflict, validation failure) are expected outcomes, not
    exceptional conditions. Throwing mixes control flow with error handling — callers have no
    way to know a method can fail without reading its body. `ErrorOr<T>` makes failure
    explicit in the return type, composes with `.Match(onValue, onError)` at the endpoint
    layer, and maps to RFC 7807 ProblemDetails without a global try-catch. Retrofitting means
    rewriting every handler and every test that currently asserts on thrown exceptions.

11. **Testing structure?**
    Options: `unit + functional (Testcontainers)` / `unit only` / `skip for now`
    Recommendation: `unit + functional (Testcontainers)`
    Why: the convention module has four projects — main, Contracts, UnitTests,
    FunctionalTests. Skipping FunctionalTests at greenfield means the first real test of a
    handler requires setting up Testcontainers, a `WebApplicationFactory<Program>` base
    class, and database seeding from scratch against existing code instead of inheriting the
    scaffold. Unit tests cover domain logic in isolation; functional tests spin a real
    database and verify the full vertical slice end-to-end. A feature is not done without
    both.

12. **OpenTelemetry from day 1?**
    Options: `OpenTelemetry from day 1` / `ILogger<T> only` / `defer`
    Recommendation: `OpenTelemetry from day 1`
    Why: the convention scaffold includes one `Infrastructure/Instrumentation.cs` per module
    with `ActivitySource` and `Meter` registered as singletons. `ILogger<T>` alone gives
    logs; OTel adds distributed traces and custom metrics — essential once the app runs in
    Azure Container Apps with Application Insights or Grafana. Deferring means retrofitting
    every module's DI registration and arriving at the first production incident without
    trace context.

13. **Input validation library?**
    Options: `FluentValidation` / `DataAnnotations` / `none`
    Recommendation: `FluentValidation`
    Why: the VSA+CQS pipeline registers a `ValidationDecorator` that runs
    `IValidator<TCommand>` before every handler — invalid commands never reach business
    logic. DataAnnotations only fires at the HTTP binding layer, not inside the handler
    pipeline, meaning a command invoked from a background job or integration test bypasses
    validation silently. FluentValidation validators are per-command, composable, and return
    all errors at once as a ProblemDetails body — never the first error only.

14. **Cross-module communication?** _(surface only when Q1 implies ≥2 business modules)_
    Options: `Contracts + domain events` / `direct module references` / `decide later`
    Recommendation: `Contracts + domain events`
    Why: the modular monolith enforces zero direct references between module implementations.
    Only `{Module}.Contracts` types (public interfaces + domain events) cross boundaries.
    Direct references compile today and fail the MOD002 Roslyn analyzer immediately. The
    Contracts project is scaffolded alongside the module — it is a one-line project that
    costs nothing upfront and makes the boundary explicit from the first cross-module call.
    Deciding later means retrofitting Contracts projects while existing call sites are already
    spread across the codebase.
