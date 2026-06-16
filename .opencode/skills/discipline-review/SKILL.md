---
name: discipline-review
user-invocable: false
description: >
  Reviews a proposed spec or existing code from BA / Architecture / QA / Security perspectives and returns a verdict.
  Model-invocable building block — orchestrated by commands (e.g. /at-validate, /at-init-constitute), not a user entry point.
  Invoked by a command when it needs to: run a multi-discipline review, gate a change, audit governance, produce a verdict.
---

> Judges, never executes. Emits per-discipline findings + one verdict. Caller decides what runs next.

## Input
- A proposed spec/change (default mode), or existing code (`--govern` mode).
- ADRs, conventions, quality gates when present.

## Phase 1 — Review by discipline

**Why:** each discipline catches failure modes the others miss; one reviewer misses cross-cutting risk.

Run four lenses, each emits findings tagged `blocker | warning | note`:

| Lens | Asks |
|------|------|
| **BA** | Intent clear? Acceptance criteria testable? Scope bounded? |
| **Architecture** | Layering/dependency direction sound? Matches ADRs? |
| **QA** | Test strategy present? Coverage + quality gates met? |
| **Security** | Auth, secrets, input validation correct? |

**Checkpoint:** findings collected from all four lenses.

## Phase 2 — Verdict

**Why:** caller needs one signal, not four opinions.

Dedup priority: `Security > QA > Arch > BA`.
Verdict: `approve | concern | block`.

**Checkpoint:** single verdict + deduped findings.

## Modes
- **default** (spec review): review a *proposed* spec → verdict.
- `--govern` (governance audit): audit *existing code* against ADRs/conventions/quality/security/drift. Full protocol in [references/governance-audit.md](./references/governance-audit.md). Ask before any follow-up.

## Output format

```
### Discipline review — <target>
| # | Severity | Discipline | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
Verdict: approve | concern | block
```

## Constraints
- Judge only — no project code writes, no command execution.
- Return findings/verdict to the caller; do not invoke other skills.
- The block→revise loop is the caller's job (re-run plan), never this skill calling plan.
