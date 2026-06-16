---
name: review-skill-quality
user-invocable: false
description: >
  Reviews a project-local skill against the quality guidelines and returns weaknesses with concrete fixes.
  Model-invocable building block — orchestrated by commands (e.g. /at-meta-improve), not a user entry point.
  Invoked by a command when it needs to: review a skill, detect skill weaknesses, apply the quality lens, score skill depth.
---

> Applies the quality lens to one skill. Returns findings + fixes. Caller decides what to change.

## Input
- One project-local skill file (or a directory of them).
- Quality standard in [references/skill-quality-guidelines/SKILL.md](./references/skill-quality-guidelines/SKILL.md).
- Canonical scaffold in [references/skill-template/SKILL.md](./references/skill-template/SKILL.md).

## Phase 1 — Apply the lens

**Why:** a skill that fails these dimensions misfires or adds no depth over the base agent.

| Dimension | Problem signal | Deletion test |
|-----------|----------------|---------------|
| Trigger | Too generic, collides with others | Would it fire without this skill? |
| Context | Acts without reading CONTEXT.md/repo | Does it act blind? |
| Steps | Vague ("implement", "do what's needed") | What exactly does it do? |
| Guardrails | No explicit "never"/"don't" | What mistake without it? |
| Handoff | No next step on finish | What does the user do next? |
| Depth | — | If deleted, does the agent still do it? Yes → shallow |

**Deletion test:** "If you delete this skill, does the agent do it the same? If yes — it's shallow."

**Checkpoint:** each dimension scored; shallow skills flagged.

## Phase 2 — Report

**Why:** caller needs concrete, prioritized fixes, not a grade.

Per skill: weakness found + concrete improvement + impact (★/★★/★★★).

**Checkpoint:** findings list with fixes ready for the caller.

## Output format

```
### Skill review — <name>
| Dimension | Verdict | Fix |
|-----------|---------|-----|
Impact: ★★★ | Depth: deep / shallow
```

## Constraints
- Reviews skills/guides only; never touches application code.
- Return findings to the caller; do not invoke other skills.
