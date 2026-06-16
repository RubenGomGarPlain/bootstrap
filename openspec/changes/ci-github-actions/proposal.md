# Proposal: CI Pipelines — GitHub Actions

## Summary

Add GitHub Actions CI workflows to the `BootStrap` repository. Currently `CI/CD: absent` (per CONTEXT.md). This change introduces a `.NET build + test` workflow and a `SPA build` workflow, running on every push to `main` and on every pull request targeting `main`.

## Scope

Cross-cutting infrastructure. No module boundaries are crossed; no bounded-context code changes.

## Decisions recorded

| Decision | Rationale |
|----------|-----------|
| SDK version from `global.json` | Repo already pins the exact SDK; CI stays in sync automatically |
| NuGet cache keyed on `Directory.Packages.props` | Central package management makes the cache key stable |
| E2E tests excluded | Playwright + Aspire tests require a fully running environment; better suited for a separate CD/integration pipeline |
| SPA as a separate job | Front-end failures surface independently from .NET failures |

## ADR trigger

No ADR required — this change is a standard CI addition with no hard-to-reverse architectural trade-offs.
