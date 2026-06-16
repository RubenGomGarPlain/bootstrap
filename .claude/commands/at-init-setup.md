---
name: at-init-setup
description: >
  One-time setup. Explores the repo, confirms the stack, then writes CONTEXT.md and generates the
  OpenSpec context config — so every other command reads persistent context instead of re-detecting.
  Use when the user asks to: set up ai-team, configure the toolkit, initialize for first use.
  Triggered by: "setup ai-team", "at-init-setup", "first time", "configure skills", "initialize toolkit".
---

> **How this command works:** preflight OpenSpec → explore → confirm → write context → (brownfield) offer governance ceremony.
> Run once per repo; re-run to refresh. Uses OpenSpec's default `spec-driven` schema as-is.

## Orchestration

1. **Preflight** — verify `openspec --version` succeeds. If absent, stop with an actionable install message (OpenSpec is a required prerequisite; the archive hook depends on it).
2. **Explore** — gather signals silently. Use `diagnose-project` for project-state detection; do not re-implement it here. Note hooks (declared in `plugin.json`) and CodeGraph (`.codegraph/`).
3. **Confirm** — summarise findings, confirm the stack with the user, preview `CONTEXT.md`.
4. **Write context** — invoke `write-context`: writes `CONTEXT.md`, generates `openspec/config.yaml:context`, and — for multi-module projects — `CONTEXT-MAP.md` and per-module `src/<module>/CONTEXT.md` glossaries. OpenSpec uses its default `spec-driven` schema; the plugin ships no custom schema.
5. **Continuation prompt** *(brownfield only)* — if code exists AND `CONSTITUTION.md` is absent, ask:
   > *"Run the constitution ceremony now to establish governance? (recommended for brownfield) [Y/n]"*
   - **Y** — prepend `<!-- draft: will be replaced by constitution ceremony -->` to `CONTEXT.md`, then proceed through `at-init-constitute` Phase 1–3 inline. The ceremony's Phase 3 writes the canonical `CONTEXT.md` (removing the draft marker).
   - **N** — skip; exit with handoff below.
   - **Greenfield** (no code yet) — skip prompt entirely; route to `/at-context-grill` instead.

## Mode: constitution
When `CONSTITUTION.md` exists, additionally render the AI rules layer (`AGENTS.md` + platform instruction files) using [../references/templates/](../references/templates/). Full protocol in [../references/constitution-mode.md](../references/constitution-mode.md).

## Handoff
`Setup complete. Run /at-status to see your first diagnosis.`
*(If ceremony ran inline: `Constitution ready. Next, run: /at-plan — enter the plan→apply loop.`)*
