# Governance audit protocol (`--govern` mode)

Full audit over existing code. **Present findings; do not apply changes automatically.**
This is distinct from default spec-review: it judges live code against governance, not a
proposed spec.

## Audit checklist (5 areas)
1. **ADR alignment** — does code match documented ADRs?
2. **Convention compliance** — does code follow engineering conventions?
3. **Quality gates** — are coverage targets, build warnings, and quality thresholds met?
4. **Security posture** — are auth patterns, secret management, and input validation correct?
5. **Drift detection** — has the project diverged from its CONSTITUTION.md?

## Findings report format

```
## Governance Audit — <project>

### Findings

| # | Severity | Area | Issue | Recommendation |
|---|----------|------|-------|----------------|
| 1 | 🔴 blocker | ... | ... | ... |
| 2 | 🟡 warning | ... | ... | ... |
| 3 | 🔵 info    | ... | ... | ... |

### Summary
- X blockers, Y warnings, Z info
- Overall: [Compliant / Needs attention / Non-compliant]
```

## Ask-before-apply contract

After the report, ask:
> Do you want me to act on any of these findings?
> 1. Yes — apply the recommended changes for the **blockers**
> 2. Pick a specific finding to work on
> 3. No — I only wanted the diagnosis

Wait for the user's choice before doing anything.

Guides used: `at-plan/references/conventions/dotnet-guardrails.md`,
`at-plan/references/conventions/engineering.md`.
