---
name: at-context-sync
description: >
  Reconciles CONTEXT.md (and the OpenSpec context config) against the last session. Thin command
  that delegates to the sync-context skill for manual invocation.
  Use when the user asks to: sync context, update context, reconcile context after a session.
  Triggered by: "sync context", "update context", "context out of date".
---

> **How this command works:** it delegates to the `sync-context` skill. The same skill is also
> triggered automatically by the `subagentStop` hook when `docs/.context-sync-needed` is present.

## Orchestration

Invoke `sync-context`. It reads the transcript flagged by the hook, patches only changed sections of `CONTEXT.md`, regenerates `openspec/config.yaml:context`, and deletes the flag. Runs silently if no delta.

## Handoff
`CONTEXT.md synced. Continue with /at-plan or /at-status.`
