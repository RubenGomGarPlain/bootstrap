---
name: write-context
user-invocable: false
description: >
  Writes all project context artifacts: root CONTEXT.md, OpenSpec context config, and — for multi-module projects — CONTEXT-MAP.md and per-module src/<module>/CONTEXT.md glossaries.
  Model-invocable building block — orchestrated by commands (e.g. /at-init-setup, /at-context-grill, /at-init-bootstrap, /at-init-constitute), not a user entry point.
  Invoked by a command when it needs to: write CONTEXT.md, persist project context, populate the OpenSpec context config, generate vertical context for multi-module projects.
---

> Single writer of all project context. Root state, OpenSpec config, context map, module glossaries — one skill owns the full context layer. Format contract for vertical artifacts in [references/context-map-format.md](./references/context-map-format.md).

## Input
- Detected/confirmed stack, hooks, decisions (from caller).
- `CONSTITUTION.md` + `docs/adr/` when present (enrich the projection).

## Phase 1 — Write root CONTEXT.md

**Why:** every command reads persistent context instead of re-detecting; one canonical file prevents drift.

Use [references/context-template.md](./references/context-template.md) as base.
Write to repo root. If `CONTEXT.md` exists, replace only the `<!-- AI-TEAM:START -->` / `<!-- AI-TEAM:END -->` block; preserve everything outside markers.
If `CONTEXT.md` contains `<!-- draft: will be replaced by constitution ceremony -->`, remove the marker — this is the canonical write.

**Checkpoint:** `CONTEXT.md` present with `## Stack`, `## Hooks`, `## CodeGraph` sections.

## Phase 2 — Generate OpenSpec context config

**Why:** OpenSpec injects `config.yaml:context` verbatim into every artifact prompt. It is inline-only (no file pointer), so it must be generated from the canonical source or it goes stale.

Derive the `context:` block of `openspec/config.yaml` from `CONTEXT.md`, enriched by `CONSTITUTION.md` + ADRs when present.
Refresh `config.yaml:rules` to match the current model (OpenSpec engine, teaching-first).
Preserve `schema:` (set by `/at-init-setup`).

**Checkpoint:** `openspec/config.yaml:context` reflects the current project, not a hand-maintained blob.

## Phase 3 — Vertical context (multi-module projects only)

**Why:** a project with ≥2 bounded contexts needs a context map and per-module glossaries so agents load context vertically, not globally.

**Module detection:** scan `src/*/` for directories containing a `*.csproj`. Exclude: `*Tests`, `*Test`, `*UnitTests`, `*IntegrationTests`, `*Specs` (test projects) and `*Host`, `*AppHost`, `*Defaults`, `*ServiceDefaults` (Aspire infra). If 0–1 modules remain, skip Phase 3 and note `(single-context — no map generated)`.

**3a — Per-module glossaries:** for each detected bounded context, infer domain terms from: module name, entity file names in `src/<module>/Domain/` when present, ADR references, `CONSTITUTION.md` domain model. Draft glossary following [references/context-map-format.md](./references/context-map-format.md). Present draft, ask developer to accept or edit. Write to `src/<module>/CONTEXT.md` — if file exists, confirm before overwriting.

**3b — Context map:** infer relationships from domain events in ADRs (emits/consumes), shared type references, Aspire AppHost registrations. Write `CONTEXT-MAP.md` at repo root following [references/context-map-format.md](./references/context-map-format.md). If no relationship signals found, write empty `## Relationships` with `<!-- Add: who emits what, who consumes what -->`.

**Checkpoint:** `CONTEXT-MAP.md` present; `src/<module>/CONTEXT.md` present for each bounded context.

## Constraints
- Writes only `CONTEXT.md`, `openspec/config.yaml`, `CONTEXT-MAP.md`, and `src/<module>/CONTEXT.md`. No application code.
- Never deletes user content outside the AI-TEAM markers.
- Never overwrites an existing module `CONTEXT.md` without confirmation.
- Return all written paths to the caller; do not invoke other skills.
