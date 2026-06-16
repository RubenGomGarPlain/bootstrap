---
name: create-pr
user-invocable: false
description: >
  Generates a pull-request title, description, and review checklist from finished work, then opens the PR.
  Model-invocable building block — orchestrated by commands (e.g. /at-apply), not a user entry point.
  Invoked by a command when it needs to: create a PR, generate PR title/description, produce a review checklist from a finished change.
---

> Turns a finished change into a reviewable PR. Reads the diff, not the spec. Caller confirms before the PR opens.

## Input
- The current branch's committed/staged work (the diff against the base branch).
- The change/spec name when available, for the title.

## Phase 1 — Summarize the diff

**Why:** a PR description grounded in the actual diff beats one paraphrased from intent.

Read the diff. Group changes by area. Identify the user-facing effect and any breaking change.

**Checkpoint:** one-paragraph summary + grouped change list.

## Phase 2 — Compose PR

**Why:** a consistent shape makes review fast and keeps history searchable.

- **Title:** imperative, scoped (`feat:` / `fix:` / `refactor:` …).
- **Description:** what + why; link the OpenSpec change when present.
- **Checklist:** tests, docs, breaking-change note, rollback.

**Checkpoint:** title + description + checklist ready.

## Phase 3 — Confirm and open

**Why:** opening a PR is outward-facing; the user authorizes it.

Show the composed PR. On go-ahead, open it with `gh pr create`. End PR body with the project's required trailer if one exists.

**Checkpoint:** PR opened; URL returned.

## Constraints
- Never force-pushes, never opens a PR without confirmation.
- Branches first if on the default branch.
- Return the PR URL to the caller; do not invoke other skills.
