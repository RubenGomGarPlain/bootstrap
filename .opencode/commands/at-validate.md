---
name: at-validate
description: >
  The gate between plan and apply. Wraps openspec validate and composes discipline-review over a
  proposed change, returning a verdict — it judges, it never executes or writes project code.
  Use when the user asks to: validate a change, review a change, run a spec review, get an agent
  review, or check a change before applying.
  Triggered by: "validate change", "review change", "spec review", "agent review", "check change".
---

> **How this command works:** it runs OpenSpec's structural validation, then composes
> `discipline-review` for the multi-discipline judgment OpenSpec does not provide. It emits a
> verdict and the next prompt — it does not call plan or apply.

## Orchestration

1. Run `openspec validate <change>` for structural validity (artifacts, scenarios, deltas).
2. Invoke `discipline-review` (default mode) over the proposed change → per-discipline findings + verdict `approve | concern | block`.
3. On `--govern`: invoke `discipline-review --govern` to audit existing code against ADRs/conventions/quality/security/drift.

## Why a verdict, not files
The block→revise loop is realised by **the user re-running `/at-plan`**, prompted by this gate's handoff — never by the gate calling plan. Keeps the no-skill-invokes-a-skill rule intact.

## Handoff
- on `approve` → `Next, run: /at-apply.`
- on `block` → `Revise first, then run: /at-plan.`
