---
name: sync-context
user-invocable: false
description: >
  Reconciles CONTEXT.md (and the OpenSpec context config) against a session transcript, patching only changed sections.
  Model-invocable building block — orchestrated by a hook or the /at-context-sync command, not a primary user entry point.
  Invoked when it needs to: sync project context after a session, reconcile CONTEXT.md drift, refresh the OpenSpec context config.
---

> Pure sync. No interview, no phases-as-conversation. Reads transcript, diffs, patches additions only, refreshes the OpenSpec config, deletes the flag.

## Input
- `docs/.context-sync-needed` (JSON with `transcriptPath`), written by the hook.
- `CONTEXT.md` at repo root.

## Steps

1. **Read flag** — read `docs/.context-sync-needed`; extract `transcriptPath`. Missing → exit silently.
2. **Read CONTEXT.md** — content between `<!-- AI-TEAM:START -->` / `<!-- AI-TEAM:END -->`. Missing CONTEXT.md → exit silently (`/at-init-setup` runs first).
3. **Read transcript** — last session's assistant turns. Extract candidates: stack changes, new ADRs, new domain guides, key decisions.
4. **Diff** — compare candidates to current CONTEXT.md. Build additions-only list. Empty → delete flag, exit silently.
5. **Patch CONTEXT.md** — append each addition to its matching section (`## Stack`, `## ADRs`, `## Domain Guides`); create section before `<!-- AI-TEAM:END -->` if absent. Preserve all existing content.
6. **Refresh OpenSpec config** — regenerate `openspec/config.yaml:context` from the patched CONTEXT.md so the injected context stays fresh.
7. **Report + clean up** — print `CONTEXT.md synced: [additions]`; delete `docs/.context-sync-needed`.

## Constraints
- Idempotent: nothing new → writes nothing.
- Never deletes existing content, never rewrites the whole file, never touches files outside `CONTEXT.md` and `openspec/config.yaml`.
- Return a summary to the caller; do not invoke other skills.
