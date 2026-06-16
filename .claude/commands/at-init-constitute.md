---
name: at-init-constitute
description: >
  Runs the multi-discipline constitution ceremony to establish the architectural contract and governance.
  Use when the user asks to: constitute the project, run the governance ceremony, define the constitution.
  Triggered by: "at-init-constitute", "constitution ceremony", "establish governance", "define architecture contract".
---

> **How this command works:** the four disciplines contribute in sequence to produce `CONSTITUTION.md`, ADRs, and AI context. Each discipline's output is reviewed before the next starts.

## Phase 1 — Discipline contributions
Walk BA → Engineering → QA → DevOps. Each produces its slice (domain model + DoR/DoD; technical design + ADRs; test strategy + quality gates + threat model; pipeline + deployment + IaC). Use governance references in [../references/governance/](../references/governance/). Invoke `write-adr` for each architectural decision.

**Checkpoint:** each discipline's output accepted before the next.

## Phase 2 — Cross-validation
Invoke `discipline-review` to challenge conflicts across the four disciplines. Any conflict reopens the relevant phase.

**Checkpoint:** no unresolved cross-discipline conflicts.

## Phase 3 — Context generation
Invoke `read-context`, then `write-context` to produce `CONSTITUTION.md`, `CONTEXT.md`, OpenSpec context config, and — for multi-module projects — `CONTEXT-MAP.md` and per-module `src/<module>/CONTEXT.md` glossaries. Invoke `write-architecture` to produce `ARCHITECTURE.md`. Render `AGENTS.md` from [../references/templates/](../references/templates/).

**Checkpoint:** constitution + context written.

## Handoff
`Constitution ready. Next, run: /at-plan — enter the plan→apply loop for the first story.`
