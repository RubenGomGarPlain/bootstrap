---
name: engineering-conventions
description: >
  Language-agnostic engineering principles for every project regardless of stack.
  Covers architecture, error handling, observability, testing strategy, CI/CD, and security.
  Reference-only — read by commands to shape decisions; generates nothing.
---

## Engineering Conventions

Company-wide principles. No scaffolding. No code generation.

For .NET-specific implementation, read `dotnet-guardrails`.

## Core Principles

| # | Principle | One-liner |
|---|-----------|-----------|
| 1 | **Repository Structure** | Standard folder layout, mandatory docs, root config files |
| 2 | **Result-Oriented Errors** | Domain errors as values (not exceptions) + RFC 7807 |
| 3 | **Explicit Boundaries** | Public interfaces between components; no coupling to internals |
| 4 | **Observability by Default** | Structured logging, distributed tracing, metrics |
| 5 | **Convention over Configuration** | Auto-discovery of handlers, validators, modules |
| 6 | **Architecture as Code** | Rules enforced via tests and linters, not just docs |
| 7 | **Everything as Code** | IaC, CI/CD, GitOps — nothing manual |
| 8 | **Code Quality & Linting** | Automated formatting + static analysis in CI — not negotiable |
| 9 | **Security by Default** | Vulnerability scanning, dependency auditing, secret detection in CI |
| 10 | **Testing Strategy** | Deliberate test pyramid: unit, integration, E2E — all automated in CI |

## Reference Documents

| Document | Topic |
|----------|-------|
| [repository-structure.md](./references/repository-structure.md) | Folder layout, README, CONTRIBUTING, ADRs, AI tooling files |
| [error-handling.md](./references/error-handling.md) | Result pattern, error factories, RFC 7807 ProblemDetails |
| [explicit-boundaries.md](./references/explicit-boundaries.md) | Public APIs, no internal coupling, dependency direction |
| [observability.md](./references/observability.md) | Structured logging, OpenTelemetry, what never to log |
| [dependency-injection.md](./references/dependency-injection.md) | Convention-based DI, pipeline decorators |
| [architecture-enforcement.md](./references/architecture-enforcement.md) | Architecture tests, linter rules, CI gates |
| [everything-as-code.md](./references/everything-as-code.md) | IaC, CI/CD, GitOps, secrets management |
| [linting.md](./references/linting.md) | Formatter, linter, CI gate, suppression rules, tooling by stack |
| [security.md](./references/security.md) | Dependency scanning, SAST, secret detection, OWASP Top 10, Zero Trust |
| [testing.md](./references/testing.md) | Test pyramid, unit tests, integration tests, E2E with Playwright |
