---
name: dotnet-guardrails
description: >
  .NET-specific guardrails covering architecture rules, build hygiene, and code quality conventions.
  Covers Directory.Build.props, central package management, editorconfig, arch tests, VSA+CQS,
  Aspire conventions, EF patterns, logging, and cross-module communication.
  Reference-only — consumed by commands to inject rationale into tasks; generates nothing.
---

## .NET Guardrails

Read `engineering-conventions` for the language-agnostic parent standard.

Reference-only. No scaffolding.

## Reference Documents

| Document | Purpose |
|----------|---------|
| [vertical-slice-cqs.md](./references/vertical-slice-cqs.md) | Feature-per-folder structure, Command/Query Separation |
| [arch-rules.md](./references/arch-rules.md) | VSA+CQS layer dependency rules |
| [arch-tests-conventions.md](./references/arch-tests-conventions.md) | Architecture test project — mandatory tests, stack, when to add more |
| [modular-monolith-guide.md](./references/modular-monolith-guide.md) | What a module is and bounded context rules |
| [cross-module-communication.md](./references/cross-module-communication.md) | Cross-module communication patterns |
| [build-defaults.md](./references/build-defaults.md) | `Directory.Build.props` and `.targets` — what to check and why |
| [central-package-management.md](./references/central-package-management.md) | `Directory.Packages.props` CPM — what to flag and common errors |
| [editorconfig-conventions.md](./references/editorconfig-conventions.md) | `.editorconfig` — naming rules, severity levels, CI enforcement |
| [di-conventions.md](./references/di-conventions.md) | Scrutor DI assembly scanning patterns |
| [aspire-conventions.md](./references/aspire-conventions.md) | Aspire resource naming and connection strings |
| [error-handling.md](./references/error-handling.md) | ErrorOr + ProblemDetails (RFC 7807) mapping |
| [logging-conventions.md](./references/logging-conventions.md) | ILogger<T> usage and log level guidelines |
| [manual-module-guide.md](./references/manual-module-guide.md) | How to add a module without the scaffold skill |
| [preflight-checks.md](./references/preflight-checks.md) | Standard pre-flight detection patterns |
| [skill-invocation-order.md](./references/skill-invocation-order.md) | Skill dependency chain and invocation guide |
| [skill-maintenance.md](./references/skill-maintenance.md) | How to update tokens, stubs, and package versions |
| [token-reference.md](./references/token-reference.md) | Master table of all `__TOKEN__` names |
