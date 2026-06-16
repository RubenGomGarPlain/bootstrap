# Plane-0 convention: engineering (language-agnostic)

> `engineering-conventions` was already a `type: reference` skill — pure knowledge, no
> scaffolding. In the 3-plane model it is **Plane-0 data**: the rung-0 conventions every
> engine reads but never executes. The full bodies live in the source `references/` set;
> this guide is the index the engine loads.

## Nature
Company-wide engineering principles, applicable to every project regardless of language or
framework. Read by `at-plan` (to shape decisions) and surfaced by `at-setup` (into the
rules layer). Never executed.

## Topics (source: `plugin/skills/engineering-conventions/references/`)
- `repository-structure.md` — folder layout, README, CONTRIBUTING, ADRs, AI tooling files.
- `error-handling.md` — Result pattern, error factories, RFC 7807 ProblemDetails.
- `explicit-boundaries.md` — public APIs, no internal coupling, dependency direction.
- `observability.md` — structured logging, OpenTelemetry, what never to log.
- `dependency-injection.md` — convention-based DI, pipeline decorators.
- `architecture-enforcement.md` — architecture tests, linter rules, CI gates.
- `everything-as-code.md` — IaC, CI/CD, GitOps, secrets management.
- `linting.md` — formatter, linter, CI gate, suppression rules, tooling by stack.
- `security.md` — dependency scanning, SAST, secret detection, OWASP Top 10, Zero Trust.
- `testing.md` — test pyramid, unit tests, integration tests, E2E with Playwright.

## Why it is data, not a skill
It generates nothing. Treating it as Plane-0 data removes the "skill that is really a
document" category and lets engines inject the relevant `references/*.md` body inline into a
`tasks.md` so the *why* is read before apply.
