# Plane-0 convention: dotnet-guardrails (.NET-specific)

> `dotnet-guardrails` was already `type: reference` (no scaffolding). In the 3-plane model
> it is **Plane-0 data**: the .NET specialisation of the engineering conventions, injected
> by engines, never executed. Full bodies live in the source `references/` set.

## Nature
.NET-specific reference for every .NET domain guide. Where a domain guide (shell/arch/...)
needs the *why* of a convention, the relevant body below is injected inline into `tasks.md`.

## Topics (source: `plugin/skills/dotnet-guardrails/references/`)
- Build hygiene: `build-defaults.md`, `central-package-management.md`, `editorconfig-conventions.md`.
- Architecture: `arch-rules.md`, `arch-tests-conventions.md`, `modular-monolith-guide.md`, `vertical-slice-cqs.md`, `cross-module-communication.md`.
- Ops: `logging-conventions.md`, `di-conventions.md`, `aspire-conventions.md`, `error-handling.md`.
- Process: `preflight-checks.md`, `skill-invocation-order.md`, `skill-maintenance.md`, `token-reference.md`, `manual-module-guide.md`.

## Why it is data, not a skill
Reference-only by its own front-matter. As Plane-0 data it feeds the engine; the protected
build files it documents (`Directory.Build.props`, `Directory.Packages.props`, `.slnx`) are
guarded by the deny-hook, so changes to them must go through a plan — matching this guide's
intent.
