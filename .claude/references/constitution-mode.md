# Constitution mode (AI context generation)

A distinct mode of `at-init-setup`, activated automatically when `CONSTITUTION.md` exists at the
repo root. Base setup writes `CONTEXT.md`; this mode additionally renders the rules layer that
makes the project visible to AI platforms.

## What this mode adds

After base setup writes `CONTEXT.md` (steps 1–4 of the main skill), render:

**`AGENTS.md`** — use [templates/AGENTS-TEMPLATE.md](./templates/AGENTS-TEMPLATE.md). Read
`CONSTITUTION.md` to populate: skill catalog, domain guides referenced.

`AGENTS.md` is a cross-runtime standard read by Claude Code, OpenAI Codex, and other AI platforms.
No additional platform-specific files are needed — `AGENTS.md` is sufficient for all runtimes.

Use [templates/SCHEMA.md](./templates/SCHEMA.md) for the shared structure the template follows.

## Activation rule

```
if CONSTITUTION.md exists at repo root:
  → run this mode after base step 4
  → write AGENTS.md from AGENTS-TEMPLATE.md + CONSTITUTION.md
  → list the additional files in step 5 output
```

## Step 5 output (constitution mode)

```
Setup complete.

Files written:
  CONTEXT.md      (stack, hooks, codegraph sections)
  AGENTS.md       (skill catalog — read by Claude Code, Copilot, OpenCode, and others)

Run at-plan --diagnose to see your first diagnosis.
Every at-* skill will now read CONTEXT.md instead of re-detecting from scratch.
```

## Place in the context stack
- **Rules / source of truth:** CONSTITUTION → CONTEXT.md / AGENTS.md (this skill).
- **Map:** CodeGraph (external).
- **Limits:** the plugin's guardrail hook (declared in `plugin.json`).
