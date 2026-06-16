# Plane-0 governance: constitution-ba (Business Analysis discipline)

> `constitution-ba` was already `type: reference`. In the 3-plane model it is **Plane-0
> data** consumed by `at-plan` during the constitution / story-refinement phase. It
> produces *analysis*, not files-by-itself, so it informs the plan rather than being an
> engine.

## Nature
The Business Analysis discipline: establishes WHAT the project delivers (domain model,
epics, stories, acceptance-criteria standards, DoR/DoD). No technical implementation — BA
owns WHAT/WHY; architecture owns HOW.

## How the engine uses it
- `at-plan` (constitution phase / `--phase story`) reads this guide to drive domain-model
  and backlog questions, then writes the analysis into the spec's README + tasks.
- `at-validate` uses the BA discipline as one of its review lenses (BA / Arch / QA / Security).

## Content (source: `skills/constitution-ba/`)
- Propose mode: identify bounded contexts → core entities/relationships → 3–5 epics with
  business value → user stories → acceptance-criteria standards → DoR/DoD.
- Review-spec mode: judge a proposed change against the BA standards.
- Template: `CONSTITUTION-TEMPLATE.md`.

## Why it is data, not a skill
Its output is a *report/analysis* feeding the plan and the gate — exactly the "produces a
verdict/analysis, not project files" category the design assigns to Plane-0 governance +
the `at-validate` gate, not to a scaffold engine.
