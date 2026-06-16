---
name: at-meta-improve
description: >
  Detects friction in the team's AI workflow and guides improvement or creation of project-local
  skills. Two modes: IMPROVE (review existing skills) and DISCOVER (surface candidates for new ones).
  Use when the user asks to: improve AI skills, review project skills, find AI workflow friction, create local skills.
  Triggered by: "improve ai skills", "review my skills", "tune ai workflow", "find ai friction", "create local skills".
---

> **How this command works:** orient on existing skills, explore the workflow, then emit an OpenSpec change for the chosen improvements. Touches only skills and guides — never application code.

## Phase 1 — Orientation
Invoke `read-context` for `## Stack`. Ask one question: do you already have project skills? → **IMPROVE** (have skills), **DISCOVER** (start fresh), or both.

## Phase 2 — Exploration
- **IMPROVE** → for each existing skill, invoke `review-skill-quality` to detect weaknesses and propose concrete fixes.
- **DISCOVER** → SDLC interview (repetitive tasks, manual conventions, biggest friction, candidate skills), one question at a time. Use [../references/skill-design-standards.md](../references/skill-design-standards.md) for the structural principles.

## Phase 3 — Propose
Present the candidates and let the user pick. Drive `opsx:propose` to emit an OpenSpec change capturing the chosen skill work.

## Handoff
`Change ready in openspec/changes/<name>. Next, run: /at-validate then /at-apply.`
