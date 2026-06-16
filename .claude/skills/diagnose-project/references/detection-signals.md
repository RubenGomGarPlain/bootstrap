# Project-state detection (single source of truth)

Shared detection signals used by `/at-init-setup`, `/at-context-grill`, and `/at-status`.
Detect once; do not re-implement per skill. If `CONTEXT.md` exists at the repo root, load its
`## Stack` / `## Hooks` / `## CodeGraph` sections and skip re-detection.

## Signals

| Signal | How to detect |
|--------|---------------|
| .NET solution | `*.sln` or `*.slnx` in root or `src/` |
| Entry-point topology | `src/**/Program.cs` |
| Architecture style | `BuildingBlocks/` with `Commands/`/`Queries/` (VSA+CQS), or `Domain/`+`Application/`+`Infrastructure/`+`Presentation/` (Clean Arch) |
| Aspire AppHost | `*.AppHost` project exists |
| Frontend | `src/frontend/` directory exists |
| IaC (Terraform) | `terraform/` directory exists |
| CI/CD pipeline | `azure-pipelines.yml` or `.github/workflows/*.yml` |
| Auth / BFF | reference to BFF, Duende, or Keycloak in the solution |
| ADRs | `docs/adr/` directory with `.md` files |
| CONSTITUTION.md | file exists at root |
| CONTEXT.md | file exists at root (setup already run?) |
| CONTEXT-MAP.md | file exists at root → multi-context (domain layer); read it for the module list |
| Module glossaries | `src/<module>/CONTEXT.md` files (the per-module ubiquitous language) |
| Project-local agents | `.github/agents/` directory with content |
| Project-local skills | `.github/skills/` directory with content |
| CodeGraph | `.codegraph/` directory; if present read `.codegraph/status.json` for staleness |

## Maturity classification

| Level | Definition |
|-------|-----------|
| **Greenfield** | No .NET solution found |
| **Bootstrapped** | Solution exists, no CONSTITUTION.md or ADRs |
| **Governed** | Solution + CONSTITUTION.md + ADRs present |
| **Mature** | Solution + constitution + (IaC or CI/CD) + project-local skills |

## Context topology

| Topology | Signal |
|----------|--------|
| **multi-context** | `CONTEXT-MAP.md` at root → report the module list from the map |
| **single-context** | root `CONTEXT.md`, no `CONTEXT-MAP.md` → no vertical domain layer (today's behaviour) |
| **none** | neither map nor module glossaries → domain layer not started |
