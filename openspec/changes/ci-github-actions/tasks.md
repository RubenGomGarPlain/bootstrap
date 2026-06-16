# Tasks: CI Pipelines — GitHub Actions

## Story
As a developer, I want CI pipelines that automatically build and test the solution on every push and PR, so broken code never reaches `main` undetected.

---

## Task 1 — Create `.github/workflows/` directory and `ci-dotnet.yml`

**File to create:** `.github/workflows/ci-dotnet.yml`

**Full content:**

```yaml
name: CI — .NET

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true

permissions:
  contents: read

jobs:
  build-and-test:
    name: Build & Test (.NET)
    runs-on: ubuntu-latest

    steps:
      - name: Checkout
        uses: actions/checkout@v4

      - name: Setup .NET (from global.json)
        uses: actions/setup-dotnet@v4

      - name: Cache NuGet packages
        uses: actions/cache@v4
        with:
          path: ~/.nuget/packages
          key: nuget-${{ hashFiles('Directory.Packages.props') }}
          restore-keys: nuget-

      - name: Restore
        run: dotnet restore BootStrap.sln

      - name: Build
        run: dotnet build BootStrap.sln --no-restore --configuration Release

      - name: Test — Unit
        if: always()
        run: dotnet test tests/TodoUnitTests --no-build --configuration Release --logger trx --results-directory TestResults

      - name: Test — Architecture
        if: always()
        run: dotnet test tests/ArchTests --no-build --configuration Release --logger trx --results-directory TestResults

      - name: Test — Functional
        if: always()
        run: dotnet test tests/FunctionalTests --no-build --configuration Release --logger trx --results-directory TestResults

      - name: Upload test results
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: test-results-dotnet
          path: TestResults/**/*.trx
```

**Acceptance check:** `dotnet build` and all three test projects exit 0.

---

## Task 2 — Create `ci-spa.yml`

**File to create:** `.github/workflows/ci-spa.yml`

**Full content:**

```yaml
name: CI — SPA

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true

permissions:
  contents: read

jobs:
  build-spa:
    name: Build SPA (React/Vite)
    runs-on: ubuntu-latest

    defaults:
      run:
        working-directory: src/spa

    steps:
      - name: Checkout
        uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: lts/*
          cache: npm
          cache-dependency-path: src/spa/package-lock.json

      - name: Install dependencies
        run: npm ci

      - name: Build
        run: npm run build
```

**Acceptance check:** `npm run build` (Vite) exits 0.

---

## Task 3 — Update CONTEXT.md

Update the `CI/CD` line in the `## Stack` section of `CONTEXT.md`:

```
**CI/CD:** present ✓ — GitHub Actions (`ci-dotnet.yml` · `ci-spa.yml`)
```

---

## Completion criteria

- [ ] `.github/workflows/ci-dotnet.yml` exists and is valid YAML
- [ ] `.github/workflows/ci-spa.yml` exists and is valid YAML
- [ ] `CONTEXT.md` CI/CD line updated to `present ✓`
- [ ] On first push, both workflows appear green in the GitHub Actions tab
