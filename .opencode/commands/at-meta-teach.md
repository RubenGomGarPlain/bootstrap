---
name: at-meta-teach
description: >
  Orients a developer joining a project. Reads the project context and explains architecture,
  conventions, decisions, and the toolkit — tailored to experience level. Teaches; never writes code.
  Use when the user asks to: onboard, get oriented, learn the codebase, understand the project, explain how things work.
  Triggered by: "onboard", "orient me", "new to this project", "explain the codebase", "teach me the project".
---

> **How this command works:** read the project's context, then walk the developer through architecture → conventions → decisions → toolkit, pausing for questions and matching depth to stated experience. Read-only.

## Process

1. **Calibrate** — ask: _"What's your experience with this stack and domain — new to both, new to one, or experienced?"_ Match depth to the answer.
2. **Read context** — invoke `read-context` for `CONTEXT.md` (`## Stack`); load `CONSTITUTION.md` and `docs/adr/`. If `CONTEXT.md` absent, say so and suggest `/at-init-setup`.
3. **Explain in four passes** (pause for questions between each): Architecture (cite ADRs) → Conventions → load-bearing Decisions → Toolkit (`/at-*` commands and when to use each).
4. **Hand off** — point to the next concrete step for what they want to do.

## Teaching stance
Every explanation states the *why*, not the *what*. Prefer the project's own ADRs and `CONTEXT.md` over generic best-practice prose.

## Handoff
`Oriented. Next, run: /at-plan to start a change, or /at-validate to review one.`
