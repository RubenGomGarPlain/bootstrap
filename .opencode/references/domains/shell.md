# Plane-0 guide: shell (.NET solution shell)

> Knowledge for the generic engine. The `dotnet-shell` + `dotnet-bootstrap` skills'
> *what/why* live here; the numbered Steps / pre-flight loop / checkbox logic belong to
> `at-apply`. Covers both the opinionated shell and the single-service bootstrap variant.

## Objective
Scaffold a runnable .NET solution shell: `.slnx`, Aspire `AppHost`, `ServiceDefaults`,
`BuildingBlocks`, `*.Web` entry-point — wired with engineering defaults so
`dotnet run --project {Project}.AppHost` boots with zero business modules. First link in
the chain.

## Choice point (non-linear) — topology
`at-plan` asks one question; the answer selects which layout is generated.

| Topology | When | Layout |
|----------|------|--------|
| `modular-monolith` (default) | one deployable, modules inside | single AppHost + BuildingBlocks + modules |
| `microservices` | multiple deployables | AppHost orchestrates N services; Docker required |
| `single-service` (bootstrap variant) | no Aspire, plain webapi/lib | `dotnet-bootstrap`: webapi/classlib/console/worker + tests |

## Inputs (with defaults)
- topology — default `modular-monolith`.
- For single-service: solution name (PascalCase), template ∈ {webapi, classlib, console, worker} (default webapi), add tests? (default yes/xunit), TFM (default net8.0), SDK pin (default `dotnet --version`).

## Pre-flight facts (engine emits as bash verify-tasks)
- .NET SDK ≥ 9.0 (shell) / ≥ 8.0 (bootstrap); Aspire workload; git; no existing `*.sln`/`*.slnx`; Docker for microservices.

## Minimum artifacts
`.slnx`, `src/{Project}.AppHost/`, `src/{Project}.ServiceDefaults/`,
`src/BuildingBlocks/{Project}.BuildingBlocks/`, `src/{Project}.Web/`,
`Directory.Build.props`, `Directory.Packages.props` (central package management),
test project, `.editorconfig`.

## Decision rationale (inline into tasks.md)
- **Aspire from day one:** local orchestration + service discovery + dashboard without per-service wiring.
- **BuildingBlocks shared by all architectures:** the arch layer (`arch.md`) plugs into it.
- **Central package management:** one source of truth for versions; arch tests enforce boundaries.
- **Shell vs bootstrap:** shell = opinionated Modular Monolith; bootstrap = generic single service. Mutually exclusive entry points, same engine.

## Bootstrap Interview (for greenfield repos)

When invoked on a greenfield repo (no `CONTEXT.md`, no `.slnx`), run the full interview
before generating a plan. Present all questions, collect answers, then generate.

**Culturization rule:** every question includes an explanation of WHAT the concept is
and WHY it matters — teach, don't just ask.

| # | Question | Options | Default |
|---|----------|---------|---------|
| 1 | **Stack** — What runtime? | dotnet (only supported today) | dotnet |
| 2 | **Architectural style** — How are deployables organized? | modular-monolith / microservices / single-service | modular-monolith |
| 3 | **Internal organization** — How is code organized inside a module? | vsa-cqs / clean-arch / minimal | vsa-cqs |
| 4 | **OpenSpec adoption** — Use spec-driven development workflow? | yes / no | yes |
| 5 | **Conventions companion** — Add engineering + .NET guardrails to AI tooling? | yes / no | yes |
| 6 | **Project name** — PascalCase identifier for namespaces and file names | (free text) | — |

After confirmation, write `docs/adr/` entries and hand off to the `at-plan → at-apply`
spine with the selected domain guides.

## What was dropped (now `at-apply`)
Pre-flight CLI checks → bash verify-tasks; "Confirm Before Proceeding" → `confirm` task;
numbered Steps → engine parse→exec loop; success summary/checkboxes → engine core.
