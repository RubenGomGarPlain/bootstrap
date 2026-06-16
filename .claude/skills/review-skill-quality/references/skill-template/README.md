# Skill Template — Front-matter Schema

## YAML Front-matter Fields

Every skill MUST include a YAML front-matter block at the top of `SKILL.md`.

| Field | Required | Type | Allowed Values | Description |
|-------|----------|------|----------------|-------------|
| `name` | Yes | string | kebab-case | Unique skill identifier (e.g., `dotnet-shell`) |
| `description` | Yes | string | multi-line | What the skill does + trigger phrases. Must include `Triggered by:` line |
| `tags` | Yes | array | strings | Categorization tags for discovery (e.g., `[dotnet, scaffold]`) |
| `type` | Yes | enum | `action`, `reference` | `action` = creates/modifies files; `reference` = read-only knowledge |
| `requires` | Yes | array | skill names | Skills that MUST have been executed before this one (empty `[]` if none) |
| `impact` | Yes | enum | `high`, `medium`, `low` | Determines confirmation behavior before execution |
| `reversible` | Yes | boolean | `true`, `false` | Whether changes can be undone. `false` requires double confirmation |
| `context-sections` | No | array | section names | Which `CONTEXT.md` sections this skill needs. Defaults to all if omitted |

## Impact Levels

| Level | Behavior | Example |
|-------|----------|---------|
| `low` | Auto-executable, no confirmation needed | Reference skills, documentation generation |
| `medium` | Show brief summary before executing | Adding a NuGet package, creating a single file |
| `high` | Full PTX plan shown, explicit confirmation required | Scaffolding solution structure, modifying architecture |

## Type Behavior

| Type | Template | Phases | Checkpoints | Outputs section |
|------|----------|--------|-------------|-----------------|
| `action` | `SKILL.md` | Yes (Phase 0–N) | Required per phase | Required |
| `reference` | `SKILL-reference.md` | No | No | No |

## Example: Action Skill Front-matter

```yaml
---
name: dotnet-shell
description: >
  Scaffolds a .NET Modular Monolith shell with Aspire, BuildingBlocks, and architecture tests.
  Use when the user asks to: create a new .NET project, bootstrap a solution, scaffold a backend.
  Triggered by: "new .NET project", "bootstrap solution", "scaffold .NET", "create backend"
tags: [dotnet, scaffold, aspire, modular-monolith]
type: action
requires: []
impact: high
reversible: true
---
```

## Example: Reference Skill Front-matter

```yaml
---
name: dotnet-guardrails
description: >
  .NET guardrails covering architecture rules, build hygiene, and code quality conventions.
  Use when the user asks to: check .NET conventions, review architecture rules, verify build config.
  Triggered by: ".NET conventions", "dotnet guardrails", "build hygiene", "arch tests"
tags: [dotnet, conventions, reference]
type: reference
requires: []
impact: low
reversible: true
---
```

## Template Files

- **Action skills:** Use [`SKILL.md`](./SKILL.md) as the starting template
- **Reference skills:** Use [`SKILL-reference.md`](./SKILL-reference.md) as the starting template

## Constraints

- Maximum total lines: **350** (extract to `references/` if larger)
- Teaching blockquotes (`> **Why:**`) required in every phase of action skills
- `description` field MUST include `Triggered by:` line for GitHub Copilot discoverability
- `references/` subdirectory for supporting documents (linked from Reference Documents table)
