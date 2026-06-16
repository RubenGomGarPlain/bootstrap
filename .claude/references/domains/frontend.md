# Plane-0 guide: frontend (SPA scaffolding)

> Knowledge from `dotnet-frontend`. A **non-linear** domain (framework choice + interactive
> `npm`), so it exercises the `confirm` task-type. Engine behaviour stripped.

## Objective
Add a TypeScript SPA under `src/frontend/`, registered as an Aspire `npm` resource in the
AppHost, with a dev proxy so `/api/*` and `/bff/*` reach `*.Web` without CORS, plus a
minimal auth hook compatible with the auth guide.

## Choice point (non-linear) — framework
| Framework | Version |
|-----------|---------|
| `react` | React 19 |
| `vue` | Vue 3 |
| `angular` | Angular 18 |

## Inputs (with defaults)
- framework — `react` | `vue` | `angular` (required).
- frontend name — folder under `src/frontend/`, default `web`.
- `__PROJECT_NAME__` inferred from `.slnx`.

## Pre-flight facts
- Shell present (`.slnx` + AppHost); Node.js ≥ 20 LTS; `src/frontend/` absent.

## Interactivity (why `confirm` matters here)
`npm install` / scaffolders (`create-vite`, `ng new`) can prompt and take time. These steps
are modelled as `confirm` tasks: `at-apply` shows the action and waits for go-ahead before
running, instead of carrying framework-specific logic.

## Decision rationale (inline into tasks.md)
- **Aspire `npm` resource:** one `dotnet run` boots backend + frontend dev server together.
- **Dev proxy not CORS:** `/api/*` and `/bff/*` proxied to `*.Web` → same-origin in dev, no CORS config.
- **Auth hook compatible with auth guide:** SPA calls `/bff/user`; session cookie does the rest.

## What was dropped (now `at-apply`)
Pre-flight checks → bash; "Confirm Before Proceeding" + interactive installs → `confirm`;
file writes/Steps → engine.
