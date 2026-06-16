---
name: at-arch-improve
description: >
  Detects existing architecture, proposes improvements, and drives the plan→apply loop with guardrails.
  Use when the user asks to: review architecture, improve structure, detect architecture patterns, fix layer violations.
  Triggered by: "at-arch-improve", "review architecture", "improve architecture", "detect patterns", "fix layers".
---

> **How this command works:** reads project context, invokes `detect-architecture`, presents findings, proposes a reviewed plan, and hands off to `/at-plan` → `/at-apply` for structural changes.

## Phase 1 — Detect
Invoke `read-context`, then `detect-architecture` to scan for patterns (VSA-CQS, Clean Architecture, minimal), layer violations, dependency-direction issues, and missing architecture tests. Use [../references/architecture-checklist.md](../references/architecture-checklist.md).

**Checkpoint:** architecture report with strengths and gaps.

## Phase 2 — Propose
Present findings. For each gap, propose a change with rationale, as a reviewable plan. Invoke `write-adr` for each architectural decision made.

**Checkpoint:** user reviews and approves.

## Phase 3 — Hand off
For each approved change, the user runs `/at-plan` (with the `arch` guide) → `/at-apply`. After approved changes land, invoke `write-architecture` to update `ARCHITECTURE.md`. This command does not call other commands.

## Escalation
Escalate to `/at-init-constitute` if governance conflicts require a ceremony ruling.

## Handoff
`Approved changes ready. Next, run: /at-plan with the arch guide, then /at-apply.`
