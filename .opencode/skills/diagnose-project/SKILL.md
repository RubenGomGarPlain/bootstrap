---
name: diagnose-project
user-invocable: false
description: >
  Scans a repo, classifies context-maturity, and returns a diagnosis with a recommended next step.
  Model-invocable building block — orchestrated by commands (e.g. /at-status, /at-plan), not a user entry point.
  Invoked by a command when it needs to: detect project state, classify maturity, recommend the next step.
---

> Read-only. Detect → classify → recommend. Never writes specs, never scaffolds. Caller decides what runs next.

## Input
- Repo as-is.
- `CONTEXT.md` at root if present — load `## Stack` / `## Hooks` / `## CodeGraph`, skip re-detection.

## Phase 1 — Detect

**Why:** one detection pass feeds every routing decision; re-detecting per caller wastes tokens.

Use signal + maturity tables in [references/detection-signals.md](./references/detection-signals.md).
Re-detect only when `CONTEXT.md` absent or caller requests refresh.

**Checkpoint:** maturity level chosen; per-signal status known.

## Phase 2 — Classify

**Why:** maturity level drives the recommendation; ambiguous level → wrong next step.

Map signals to level: Greenfield / Bootstrapped / Governed / Mature (table in references).
Name the single biggest gap in one line.

Also classify **context topology** (table in references): `CONTEXT-MAP.md` present → multi-context (report the module list from the map); root `CONTEXT.md` only → single-context; neither → domain layer not started. Callers use this to decide whether context loads vertically.

**Checkpoint:** one level, one biggest-gap line, one topology.

## Phase 3 — Recommend (priority order)

**Why:** caller needs one clear next step, not a menu.

1. No `CONTEXT.md` → `/at-init-setup`
2. No solution → `/at-context-grill`
3. Solution, no `CONSTITUTION.md` → `/at-init-constitute`
4. Governed/Mature → `/at-plan` (steady loop)
5. Repeated pattern worth a skill → `/at-meta-improve`
6. Any project → `/at-validate` always valid
7. Mature, all green → close with `_"I love it when a plan comes together."_`

**Checkpoint:** one recommended step + lower-priority alternatives.

## Output format

```
### Diagnosis  (where are we?)
Maturity: <Level>
Topology: <multi-context | single-context | none>  (modules: <list if multi>)
| Signal | Status |
|--------|--------|
| ...    | ✓ Found / ✗ Missing |
Biggest gap: <one line>

### Recommended next step
**<command>** — <reason tied to biggest gap>. Nothing runs yet.
Alternatives: <command> — <reason>
```

## Constraints
- Read-only. No file writes, no commands.
- Return the diagnosis to the caller; do not invoke other skills.
