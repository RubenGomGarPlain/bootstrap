---
name: at-init-bootstrap
description: >
  Runs the greenfield bootstrap interview and scaffolds a .NET solution with ADRs and CONTEXT.md.
  Use when the user asks to: start a new project, scaffold a solution, bootstrap a greenfield.
  Triggered by: "at-init-bootstrap", "new project", "start from scratch", "bootstrap"
---

> **How this command works:** structured interview → scaffold via `scaffold-dotnet-solution` → record decisions and context. Recommended by `/at-status` when no .NET solution is detected.

## Phase 1 — Interview
Ask one at a time, show defaults: stack (`dotnet`), architectural style (modular-monolith / microservices / single-service), internal organization (vsa-cqs / clean-arch / minimal), conventions companion (yes/no), project name (PascalCase). Show a summary and confirm.

**Checkpoint:** all answers confirmed.

## Phase 2 — Scaffold
Invoke `scaffold-dotnet-solution`, passing the confirmed answers plus the `shell` and `arch` guides from [../references/domains/](../references/domains/). It lays down structure and layers and verifies the build.

**Checkpoint:** `dotnet build` passes.

## Phase 3 — Record
Invoke `write-adr` for each architectural decision made. Invoke `write-context` to write `CONTEXT.md` and the OpenSpec context config.

**Checkpoint:** `CONTEXT.md` and at least one ADR exist.

## Handoff
`Solution scaffolded. Next, run: /at-init-constitute to formalize governance, or /at-status to reassess.`
