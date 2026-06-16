---
name: at-plan
description: >
  Interview → reviewable OpenSpec change. Runs a .NET-aware interview, loads the relevant domain
  guide, then drives opsx:propose to emit a reviewable change — without touching code.
  Use when the user asks to: plan a change, refine a story, propose a change, prepare spec-driven
  development, or start any scaffolding flow.
  Triggered by: "plan", "refine story", "propose change", "prepare sdd", "new project",
  "add architecture", "add auth", "add frontend", "add iac", "add cicd", "add aspire".
---

> **How this command works:** it conducts the .NET interview, injects the relevant domain guide,
> then drives `opsx:propose` to produce an OpenSpec change under `openspec/changes/`. It never
> scaffolds code or runs commands — that is `/at-apply`'s job. OpenSpec is the spec engine; this
> command is the .NET-aware front-end to it (no bespoke `docs/specs/NNN` flow).

## Orchestration

1. Invoke `read-context` before planning, passing the **change scope** (target module/paths) for a vertical load — root project-state + only the touched module's glossary + its ADRs. If `read-context` returns `candidate_contexts` (ambiguous scope), ask the user which context applies before continuing. If `CONTEXT.md` absent, invoke `diagnose-project` and suggest `/at-context-grill`.
2. Conduct the interview — one question at a time, show `> Recommended: …`, offer "EXPLAIN ME MORE". Load the relevant domain guide from `docs/domain-guides/` first (project-local), falling back to [../references/domains/](../references/domains/); conventions from [../references/conventions/](../references/conventions/); governance from [../references/governance/](../references/governance/). Project-local always wins.
3. Drive `opsx:propose` to emit the OpenSpec change (proposal/specs/design/tasks) under `openspec/changes/`.
4. When the plan makes a decision that passes the ADR bar (hard to reverse ∧ surprising without context ∧ a real trade-off), invoke `write-adr` at approval time. `/at-apply` executes; it does not decide.

## Non-linear domains
For domains with a choice point (shell topology, arch style, iac target, frontend framework), ask the question here and bake the answer into the change's tasks.

## Handoff
`Change ready in openspec/changes/<name>. Next, run: /at-validate then /at-apply.`
