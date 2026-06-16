---
name: detect-architecture
user-invocable: false
description: >
  Scans a .NET repo and returns a structured architecture report.
  Model-invocable building block — orchestrated by commands (e.g. /at-status, /at-arch-improve), not a user entry point.
  Invoked by a command when it needs to: detect architecture, scan for patterns, identify layers.
---

> **How this skill works:** Single-pass scan of the repo. Returns structured report. Does not modify files.

## Input

- Repo root path (defaults to CWD)

## Scan targets

| What | Where |
|------|-------|
| Solution structure | `*.sln`, `src/` layout |
| Architecture pattern | `BuildingBlocks/`, `Commands/`, `Queries/`, Clean layers |
| Dependency direction | Project references in `*.csproj` |
| Architecture tests | `*.ArchTests.csproj` or `NetArchTest` references |
| IaC | `terraform/` directory |
| CI/CD | `azure-pipelines.yml`, `.github/workflows/` |

## Output format

```
Architecture: <pattern-name>
Layers: [list]
Violations: [list or "none detected"]
Missing: [list of expected elements not found]
Strengths: [list]
```

## Constraints

- Read-only — no file writes
- Returns empty report if no `.sln` found (greenfield signal)
