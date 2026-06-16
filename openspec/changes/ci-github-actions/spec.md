# Spec: CI Pipelines — GitHub Actions

## Problem

`CI/CD: absent` — the repository has no automated build or test gate. Pull requests can merge with broken builds or failing tests.

## Goal

Introduce two GitHub Actions workflows that run on `push` to `main` and on `pull_request` targeting `main`:

1. **`ci-dotnet.yml`** — build the .NET solution and run unit, architecture, and functional tests.
2. **`ci-spa.yml`** — install Node dependencies and run the Vite production build for the React SPA.

## Out of scope

- E2E tests (`tests/E2ETests/`) — require Aspire orchestration; deferred to a CD pipeline.
- Docker image build or push.
- Deployment to any environment.

## Acceptance criteria

- `dotnet build` exits 0 on a clean checkout.
- `TodoUnitTests`, `ArchTests`, and `FunctionalTests` pass (or the workflow fails and blocks merge).
- `vite build` exits 0 for the SPA.
- NuGet packages are cached between runs (cache key: hash of `Directory.Packages.props`).
- SDK version is resolved from `global.json` automatically (`setup-dotnet` with no explicit version override).
- Both workflows appear in the GitHub Actions tab and produce a green checkmark on a passing run.
