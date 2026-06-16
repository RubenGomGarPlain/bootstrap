---
name: at-apply
description: >
  Reviewable OpenSpec change → reality. Wraps opsx:apply to execute the change's tasks, then
  composes create-pr. Knows nothing about .NET — all knowledge lives in the change.
  Use when the user asks to: apply a change, implement a plan, run the pending tasks, or create
  a PR from finished work.
  Triggered by: "apply", "implement", "run the plan", "execute change", "create PR", "make PR".
---

> **How this command works:** it wraps `opsx:apply` to execute the next pending OpenSpec change
> task by task (verify, self-repair, mark complete), then composes `create-pr`. It never invents
> tasks and never assumes a stack — all knowledge lives in the change. Always run after a plan exists.

## Orchestration

1. Drive `opsx:apply` for the target OpenSpec change — it executes each task, verifies, and marks progress. Stop between changes; never auto-chain.
2. On `--phase pr` (or when the change is complete): invoke `create-pr` to generate the PR title/description/checklist and open it after confirmation.

## Scope boundary
`/at-apply` owns task execution + the PR phase. Archiving a finished change is handled by the auto-archive hook (`subagentStop` → `openspec archive`), not here.

## Handoff
`Change applied. Next, run: /at-apply for the next change, or /at-status to reassess.`
