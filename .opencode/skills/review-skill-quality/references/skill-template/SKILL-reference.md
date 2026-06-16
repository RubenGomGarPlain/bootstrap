---
name: your-reference-skill-name
description: >
  Brief description of what conventions or knowledge this skill provides.
  Use when the user asks to: check conventions, review rules, understand patterns.
  Triggered by: "phrase 1", "phrase 2", "phrase 3"
tags: [tag1, tag2, reference]
type: reference
requires: []
impact: low
reversible: true
---

## Overview

This skill provides reference documentation for [topic]. It is **read-only** — it does not
scaffold or generate any code. Other skills load this reference with `Read @skill:<name>`.

For the language-agnostic parent standard, read `@skill:engineering-conventions`.

## Reference Documents

| Document | Purpose |
|----------|---------|
| [topic-a.md](./references/topic-a.md) | Description of what this reference covers |
| [topic-b.md](./references/topic-b.md) | Description of what this reference covers |
| [topic-c.md](./references/topic-c.md) | Description of what this reference covers |

## Quick Reference

| # | Rule | One-liner |
|---|------|-----------|
| 1 | **Rule Name** | Brief explanation of the rule |
| 2 | **Rule Name** | Brief explanation of the rule |
| 3 | **Rule Name** | Brief explanation of the rule |

## Skill Dependencies

```
this-skill              ← no dependencies (standalone reference)
dependent-skill-a       ← reads this-skill
dependent-skill-b       ← requires dependent-skill-a output
```
