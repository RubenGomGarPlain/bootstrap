---
name: read-context
user-invocable: false
description: >
  Reads CONTEXT.md and returns structured project context for use by commands.
  Model-invocable building block — orchestrated by commands (e.g. /at-status, /at-plan), not a user entry point.
  Invoked by a command when it needs to: load project context, read CONTEXT.md, get project info.
---

> **How this skill works:** Reads project context once and returns structured output. Loads **vertically** — root project-state always, plus only the module context(s) a change touches. Commands call this instead of re-reading context themselves — single source of truth, no token duplication.

## Input

- Path to root `CONTEXT.md` (defaults to `./CONTEXT.md`).
- Optional **scope**: the module, paths, or topic a change touches. Drives the vertical load.

## Vertical load

**Why:** loading every module's context wastes tokens and blurs the change's actual domain. Load the slice the change touches, not the whole repo.

1. Always load the root `CONTEXT.md` (project-state). Read only the sections the caller needs.
2. If `CONTEXT-MAP.md` exists, infer the relevant context(s) from the scope (map format in [references/context-map-format.md](./references/context-map-format.md)). Load only those `src/<module>/CONTEXT.md` glossaries + their `docs/adr/`.
3. Cross-boundary: pull a directly-related neighbour's glossary **only** when the map's `## Relationships` say the change crosses that boundary. Never load unrelated contexts.
4. **Ambiguous scope** (matches several contexts or none) → return `candidate_contexts` and stop; the caller asks the user. Never guess silently.
5. No `CONTEXT-MAP.md` → load only the root `CONTEXT.md` (identical to pre-vertical behaviour).

## Output

```
project_name: <string>
stack: <string>
architecture: <pattern>
modules: [list]
constitution_present: true/false
adrs: [list of titles]
platforms: [copilot, opencode, cursor, gemini — detected]
topology: multi-context | single-context | none
contexts_loaded: [list of module contexts actually loaded]   # the vertical slice
candidate_contexts: [list]                                    # set only when scope is ambiguous
```

## Constraints

- Read-only.
- Returns `context_missing: true` if root `CONTEXT.md` not found (signal for /at-status to recommend /at-context-grill).
- Reports `contexts_loaded` so the vertical slice is visible. Surfaces ambiguity via `candidate_contexts` — does not guess.
- Does not infer or hallucinate missing fields — returns null for absent keys. Does not invoke other skills.
