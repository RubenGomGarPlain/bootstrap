---
name: write-architecture
user-invocable: false
description: >
  Writes ARCHITECTURE.md at repo root from scanned or interviewed architecture context.
  Covers architecture style, key patterns, layering rules, and ADR index.
  Invoked by commands after architecture scan or interview; never invoked directly by users.
---

> **Single output:** `ARCHITECTURE.md` at repo root. No stack summary — that lives in `CONTEXT.md`. No ADR content — links only.

## Phase 1 — Gather

Read, in order:
1. `CONTEXT.md` — for project name and module list (reference only; do not copy stack content)
2. `docs/adr/` — scan all ADR files; extract: decision title, status (accepted/superseded), date
3. Interview output or detect-architecture report passed by the caller — architecture style, patterns, layer boundaries, known violations

If `ARCHITECTURE.md` already exists, read it and preserve content outside `AI-TEAM:AUTO-START/END` markers.

**Checkpoint:** architecture style identified; ADR list compiled.

## Phase 2 — Write

Write `ARCHITECTURE.md` using [./references/architecture-template.md](./references/architecture-template.md).

Inside `AI-TEAM:AUTO-START/END` markers, populate:

- **Architecture Style** — primary style (VSA-CQS, Clean Architecture, Minimal, etc.) + rationale in one sentence
- **Key Patterns** — bullet list of patterns in use (CQRS, Mediator, Repository, Outbox, etc.)
- **Layering Rules** — dependency direction rules; what layers exist; what crosses boundaries and what must not
- **ADR Index** — table of all ADRs from `docs/adr/`: number, title, status, link

Do not include stack (runtime, frameworks, cloud) — already in `CONTEXT.md`.
Do not include domain model or team process — belong in `CONSTITUTION.md`.

**Checkpoint:** `ARCHITECTURE.md` written; ADR index entries match files in `docs/adr/`.

## Constraints

Return the path `ARCHITECTURE.md` to the caller. Do not invoke other skills.
