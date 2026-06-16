---
name: write-adr
user-invocable: false
description: >
  Writes a single Architecture Decision Record file to docs/adr/.
  Model-invocable building block — orchestrated by commands (e.g. /at-arch-improve, /at-init-constitute), not a user entry point.
  Invoked by a command when it needs to: write an ADR, document an architectural decision, record a decision.
---

> **How this skill works:** Takes one decision as input, writes one numbered ADR file. Single responsibility — does not validate or chain decisions.

## Input

- `title` — short decision title
- `context` — why this decision was needed
- `decision` — what was decided
- `consequences` — trade-offs and implications

## Output

File written to `docs/adr/NNN-<kebab-title>.md` where NNN is the next available number.

## ADR format

```markdown
# NNN. <Title>

Date: <YYYY-MM-DD>
Status: Accepted

## Context
<context>

## Decision
<decision>

## Consequences
<consequences>
```

## Constraints

- One ADR per invocation
- Never overwrites an existing ADR — increments number if conflict
- Creates `docs/adr/` if it does not exist
