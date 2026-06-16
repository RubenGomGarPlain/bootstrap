# Guardrails Format Reference

## Full Schema

```yaml
requires:
  files: [list of file paths that must exist]
  branch_pattern: "regex pattern current branch must match"
  env: [list of environment variable names that must be set]
  clean_worktree: true|false
```

## Field Reference

### `files`
List of file paths (relative to project root) that must exist before the skill runs.

```yaml
requires:
  files:
    - "CONTEXT.md"     # Context must be harvested first
    - "openspec/config.yaml"    # OpenSpec must be initialized
```

**Use when:** The skill reads or extends files that must pre-exist.

### `branch_pattern`
Regular expression that the current git branch name must match.

```yaml
requires:
  branch_pattern: "^(feature|fix|chore|refactor)/"
```

Common patterns:
| Pattern | Matches |
|---------|---------|
| `^(feature\|fix\|chore)/` | Standard prefixed branches |
| `^(?!main\|master\|develop)` | Any non-main branch |
| `^feature/` | Feature branches only |
| `.` | Any branch (effectively no check) |

**Use when:** The skill creates commits or PRs that should not land directly on main.

### `env`
List of environment variable names that must be set (non-empty).

```yaml
requires:
  env:
    - "GITHUB_TOKEN"
    - "SONAR_TOKEN"
```

**Use when:** The skill calls external APIs or services requiring authentication.

### `clean_worktree`
When `true`, requires no uncommitted changes in the working tree.

```yaml
requires:
  clean_worktree: true
```

**Use when:** The skill generates files and you want to clearly see what it produced (no noise from uncommitted changes).

## Examples by Skill Type

### Action skill (modifies files):
```yaml
requires:
  files: ["CONTEXT.md"]
  branch_pattern: "^(feature|fix|chore)/"
  clean_worktree: true
```

### Reference skill (read-only):
```yaml
requires: {}  # No prerequisites
```

### Skill requiring CI tokens:
```yaml
requires:
  env: ["GITHUB_TOKEN"]
  branch_pattern: "^(feature|fix)/"
```

### Irreversible skill (must combine with reversible: false):
```yaml
reversible: false
requires:
  files: ["CONSTITUTION.md"]
  clean_worktree: true
```

## Bypass

```
@skill:my-skill --skip-guardrails
```

Always emits:
```
⚠ Guardrail checks bypassed. Proceeding without validation.
   Reason to bypass should be intentional and documented.
```

---

## Documentation Requirements

> **Rule:** Any skill with `impact: high` or `impact: medium` that makes an architectural,
> infrastructure, or technology decision **MUST** include a documentation phase as its final step.

### What to document

| Decision type | Required output |
|---|---|
| Architecture choice (style, patterns, topology) | ADR in `docs/adr/ADR-{NNN}-{slug}.md` |
| Technology choice (framework, DB, auth provider) | ADR in `docs/adr/ADR-{NNN}-{slug}.md` |
| Infrastructure choice (deploy target, region, IaC) | ADR in `docs/adr/ADR-{NNN}-{slug}.md` |
| Project creation (new solution, new module) | `README.md` update or creation |
| DevOps setup (pipeline, environments) | ADR + brief runbook in `docs/runbooks/` |

### ADR format (MADR lightweight)

```markdown
# ADR-{NNN}: {Title}

## Status
Accepted

## Context
{Why this decision was needed — 2-3 sentences}

## Decision
{What was decided — 1-2 sentences}

## Alternatives Considered
| Option | Pros | Cons |
|--------|------|------|
| {chosen} ✅ | ... | ... |
| {alternative} | ... | ... |

## Consequences
- {Positive consequence}
- {Negative consequence or trade-off}
```

### How to number ADRs

1. Scan `docs/adr/` for existing files
2. Find the highest `{NNN}` number
3. Use `{NNN + 1}` for the new ADR
4. If `docs/adr/` does not exist, create it and start at `001`

### CONTEXT.md update

If `CONTEXT.md` exists at project root, update the relevant section (Stack, Patterns, Decisions, Constraints) to reflect the new decision. Keep entries brief (one line per decision in the Decisions table).
