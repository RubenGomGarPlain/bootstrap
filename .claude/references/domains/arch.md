# Plane-0 guide: arch (internal architecture layer)

> Knowledge from `dotnet-arch`. Adds internal architecture to `BuildingBlocks`. Engine
> behaviour stripped; what/why kept.

## Objective
Add internal architecture infrastructure to `BuildingBlocks`, in one of two mutually
exclusive styles chosen before any module is scaffolded. The shell (`shell.md`) is shared
by both.

## Choice point (non-linear) — architecture style
| Style | Provides |
|-------|----------|
| `vsa` (VSA + CQS, default) | Command/Query interfaces, dispatcher, pipeline behaviours |
| `clean-arch` | `IUseCase`, `IUnitOfWork`, `IRepository`, decorator pipeline |

Mutually exclusive: the solution picks one; every later module follows it.

## Inputs
- style — `vsa` (default) or `clean-arch`.
- `__PROJECT_NAME__` inferred from `.slnx`.

## Pre-flight facts
- Shell present: `*.slnx` + `src/BuildingBlocks/{Project}.BuildingBlocks/` (else route to shell guide).

## Decision rationale (inline into tasks.md)
- **One style per solution:** mixing CQS and Clean handlers fragments the codebase and breaks arch tests.
- **VSA + CQS default:** vertical slices keep features cohesive; CQS separates read/write concerns and makes pipeline behaviours (validation, logging, tx) composable.
- **Clean Arch alternative:** `IUseCase` + decorators for teams preferring ports-and-adapters / Onion.
- **Plugs into BuildingBlocks:** so the choice is solution-wide and discovered by module scaffolding.

## References (the *why*, injected before apply)
`vertical-slice-cqs.md`, `arch-rules.md`, `persistence-by-architecture.md` (from the
dotnet-guardrails / engineering conventions data).

## What was dropped (now `at-apply`)
Pre-flight checks → bash; numbered Steps + file writes → engine; checkboxes/summary → engine.
