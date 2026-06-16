# Design: CI Pipelines — GitHub Actions

## Workflow 1 — `ci-dotnet.yml`

**File:** `.github/workflows/ci-dotnet.yml`

**Triggers:**
```yaml
on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
```

**Runner:** `ubuntu-latest`

**Steps:**
1. `actions/checkout@v4`
2. `actions/setup-dotnet@v4` — no `dotnet-version` override; reads `global.json` automatically
3. `actions/cache@v4` — cache `~/.nuget/packages`, key = `nuget-${{ hashFiles('Directory.Packages.props') }}`
4. `dotnet restore BootStrap.sln`
5. `dotnet build BootStrap.sln --no-restore --configuration Release`
6. `dotnet test tests/TodoUnitTests --no-build --configuration Release --logger trx`
7. `dotnet test tests/ArchTests --no-build --configuration Release --logger trx`
8. `dotnet test tests/FunctionalTests --no-build --configuration Release --logger trx`

**TRX upload:** Use `actions/upload-artifact@v4` to upload `**/*.trx` so test results are visible in the run summary.

---

## Workflow 2 — `ci-spa.yml`

**File:** `.github/workflows/ci-spa.yml`

**Triggers:** same as above.

**Runner:** `ubuntu-latest`

**Steps:**
1. `actions/checkout@v4`
2. `actions/setup-node@v4` — `node-version: lts/*`; `cache: npm`; `cache-dependency-path: src/spa/package-lock.json`
3. `npm ci` (in `src/spa/`)
4. `npm run build` (in `src/spa/` — Vite production build)

---

## Permissions

Both workflows declare `permissions: contents: read` at workflow level (least-privilege — GITHUB_TOKEN write access is not needed for CI).

## Test step resilience

All three `dotnet test` steps carry `if: always()` so Arch and Functional test results are collected even when Unit tests fail. The `upload-artifact` step also uses `if: always()`.

## Cache strategy

| Cache | Key | Restore key |
|-------|-----|-------------|
| NuGet | `nuget-${{ hashFiles('Directory.Packages.props') }}` | `nuget-` |
| npm | `node-${{ hashFiles('src/spa/package-lock.json') }}` | `node-` (managed by setup-node) |

## Concurrency

Add a `concurrency` group per-workflow to cancel superseded runs on the same PR branch:

```yaml
concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true
```

This prevents queued runs from stacking up on a fast-moving PR.
