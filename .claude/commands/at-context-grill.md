---
name: at-context-grill
description: >
  Deep architecture interview + context generator for .NET projects, greenfield or brownfield.
  Grills architecture decisions, scans existing code when present, and writes CONTEXT.md,
  ARCHITECTURE.md, ADRs, and domain guides. Usable at any project stage — not just initial setup.
  Use when the user asks to: document architecture, grill decisions, deep scan a project, add a module context.
  Triggered by: "grill decisions", "document architecture", "deep scan", "add module context", "scan my project".
---

> **How this command works:** detect greenfield vs brownfield, adapt the flow (interview vs scan + validation), and produce `CONTEXT.md`, `ARCHITECTURE.md`, `docs/adr/`, `docs/domain-guides/`. Never touches application code.

<posture>
Interview one question at a time. Wait for the answer before continuing. For each question, give the recommended answer and explain WHY it matters. If `CONTEXT.md` already exists, read it first and skip answered questions.
</posture>

## Phase 0 — Mode detection
Invoke `diagnose-project` for project-state detection:
- Empty repo or scaffolding only → **GREENFIELD**
- Existing application code → **BROWNFIELD** (also invoke `detect-architecture` to map the existing structure)

Announce the detected mode.

## Phase 1 — Flow by mode
- **GREENFIELD** → run [../references/grill/greenfield-interview.md](../references/grill/greenfield-interview.md)
- **BROWNFIELD** → run [../references/grill/brownfield-scan.md](../references/grill/brownfield-scan.md)

## Phase 2 — Document
Invoke `write-context` to write `CONTEXT.md`, the OpenSpec context config, and — for multi-module projects — `CONTEXT-MAP.md` and per-module `src/<module>/CONTEXT.md` glossaries. Invoke `write-architecture` to write `ARCHITECTURE.md`. Write `docs/adr/` and `docs/domain-guides/<project>.md` using [../references/grill/output-templates.md](../references/grill/output-templates.md).

## Handoff
`Context written. Next, run: /at-plan to enter the loop. Once a pattern repeats, /at-meta-improve.`
