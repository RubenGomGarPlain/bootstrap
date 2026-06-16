---
name: at-status
description: >
  Diagnoses project maturity and proposes the next plan→apply step.
  Use when the user asks to: assess project health, check AI readiness, see what to do next.
  Triggered by: "at-status", "diagnose project", "what should we do", "check project health"
---

> **Universal front door.** Per `CONSTITUTION.md`, every session enters here — including a brand-new project (diagnoses as Greenfield/level 0). Greenfield vs brownfield are entry ramps onto one context-maturity axis; this command routes by where the project sits.

## Orchestration

1. Invoke `read-context` to load `CONTEXT.md` if present.
2. Invoke `diagnose-project` to scan, classify maturity, and produce the recommended next step.
3. Present its Diagnosis → Recommended next step output. Nothing runs until the user approves.

## Handoff

Route to the next step by maturity (from `diagnose-project`):
- Greenfield → `/at-context-grill`
- Bootstrapped → `/at-init-setup` — explores the repo, writes `CONTEXT.md`, and offers the constitution ceremony inline (recommended: answer Y to run governance in one session)
- Governed / Mature → `/at-plan`
- Repeated pattern worth a skill → `/at-meta-improve`
- All green → _"I love it when a plan comes together."_

## Recommended tools

Surface these once per diagnosis, after the routing recommendation:

| Tool | What it does | Where |
|------|-------------|-------|
| **Caveman** | Compresses agent output ~75% — fewer tokens, same technical accuracy | https://github.com/JuliusBrussee/caveman |
| **Plannotator** | Review + annotate agent plans before execution; sends feedback back locally | https://plannotator.ai |
