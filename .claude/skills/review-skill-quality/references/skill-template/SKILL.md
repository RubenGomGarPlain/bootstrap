---
name: your-skill-name
description: >
  Brief description of what this skill does. Include trigger phrases for discoverability.
  Use when the user asks to: action 1, action 2, action 3.
  Triggered by: "phrase 1", "phrase 2", "phrase 3"
tags: [tag1, tag2]
type: action
requires: []
impact: medium
reversible: true
---

> **How this skill works:** One sentence explaining what this skill produces and how it operates.

## Target structure

```text
output-directory/
├── file1
├── file2
└── subdirectory/
    └── file3
```

---

## Phase 0 — Preflight

> **Why:** Verifying preconditions before making changes prevents partial scaffolding failures
> and wasted time debugging environment issues.

Verify the environment before creating anything.

```bash
# example preflight commands
command --version   # must be X.Y or later
test ! -d target   # target directory must not exist
```

- Condition not met → stop and inform the user with remediation steps.

**Checkpoint:** All preconditions satisfied; safe to proceed.

---

## Phase 1 — Core Implementation

> **Why:** Explain why this phase exists and what value it provides. 2-3 sentences of rationale
> that help a developer without prior context understand the design decision.

Steps to execute the main work of this skill.

```bash
# example commands
```

**Checkpoint:** Describe verifiable condition (e.g., "file X exists and contains Y").

---

## Phase 2 — Configuration

> **Why:** Rationale for this configuration phase.

Apply configuration using these references:

| File | Reference |
|------|-----------|
| `target-file` | [reference-name.md](references/reference-name.md) |

**Checkpoint:** Describe verifiable condition.

---

## Phase 3 — Verification

> **Why:** Running the full build/test cycle ensures no silent breakage was introduced.
> Never report success without this verification passing.

```bash
# verification commands (build, test, lint, etc.)
```

**Checkpoint:** All verification commands pass with zero errors.

---

## Outputs

| File | Purpose |
|------|---------|
| `path/to/file1` | Description of what this file does |
| `path/to/file2` | Description of what this file does |

---

## Troubleshooting

| Symptom | Cause | Action |
|---------|-------|--------|
| Error message X | Common cause | Remediation step |
| Error message Y | Common cause | Remediation step |

---

## Guardrails Middleware

> Every skill should declare its prerequisites in front-matter. The guardrails section runs
> before Phase 0 to ensure the execution environment is valid.

### Checking Prerequisites

At skill load time, validate all declared `requires` fields:

```bash
# files check
test -f "CONTEXT.md"   # if requires.files includes CONTEXT.md

# branch_pattern check
git rev-parse --abbrev-ref HEAD | grep -E "^(feature|fix|chore)/"

# env check
test -n "$GITHUB_TOKEN"

# clean_worktree check
test -z "$(git status --porcelain)"
```

### Violation Reporting

If any prerequisites fail, report ALL failures at once (never fail-fast one-by-one):

```
⚠ Guardrail violations (2):
  1. Missing file: CONTEXT.md
     → Run: @skill:context-harvester

  2. Branch pattern mismatch: "main" does not match "^(feature|fix|chore)/"
     → Run: git checkout -b feature/your-feature-name

Fix the above before running this skill.
Bypass (advanced): add --skip-guardrails to your invocation.
```

### Bypass and Irreversible Skills

- `--skip-guardrails`: Bypasses checks with a visible warning. Use only when you know the environment is valid.
- `reversible: false` skills: Always require double confirmation, even with `--skip-guardrails`:

```
⚠ This skill is IRREVERSIBLE. Changes cannot be undone.
Type "I understand this cannot be undone" to continue:
```

### requires Front-matter Format

```yaml
requires:
  files:
    - "CONTEXT.md"           # Must exist
    - ".openspec/config.yaml"         # Must exist
  branch_pattern: "^(feature|fix|chore)/"  # Current branch must match
  env:
    - "GITHUB_TOKEN"                   # Must be set
  clean_worktree: true                 # No uncommitted changes
```

All fields are optional. Omit `requires` entirely for skills with no prerequisites.

See: `skills/skill-template/references/guardrails-format.md` for full reference.
