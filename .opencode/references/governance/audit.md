# Plane-0 governance: audit (Governance Audit for at-validate --govern)

> Knowledge consumed by `at-validate` (or `at-plan --phase govern`) when the user
> invokes `at-validate --govern`. Defines the three-phase scan-report-ask protocol.
> **Do not apply changes without user confirmation.**

## Protocol

### Phase 1 — Scan

Check all five areas:

1. **ADR alignment** — Do code patterns match documented ADRs in `docs/adr/`?
2. **Convention compliance** — Does code follow engineering conventions (`engineering.md`, `dotnet-guardrails.md`)?
3. **Quality gates** — Are coverage targets, build warnings, and quality thresholds met (`.editorconfig`, `Directory.Build.props`)?
4. **Security posture** — Are auth patterns, secret management, and input validation correct?
5. **Drift detection** — Has the project diverged from its `CONSTITUTION.md`?

Adapt scope to what exists:
- **No CONSTITUTION.md** → generic best practices only
- **CONSTITUTION.md exists** → enforce specific quality contracts from governance
- **ADRs exist** → check alignment between code and documented decisions

### Phase 2 — Report

Produce a structured findings table:

| # | Area | Severity | Finding | Recommendation |
|---|------|----------|---------|----------------|
| 1 | … | 🔴 blocker / 🟡 warning / 🔵 info | … | … |

### Phase 3 — Ask before acting

After presenting the report, present the menu and **wait for the user's answer before doing anything**:

```
¿Quieres que actúe sobre alguno de estos hallazgos?
1. Sí, aplica los cambios recomendados para los **blockers**
2. Elige un hallazgo específico para trabajar
3. No, solo quería el diagnóstico
```

If the user selects option 1 or 2, proceed item by item, confirming before each change.

## Maturity Behavior

| Maturity | Behavior |
|----------|----------|
| Greenfield | Minimal — only generic best practices apply |
| Bootstrapped | Check build hygiene, suggest constitution for formal gates |
| Governed | Full enforcement of CONSTITUTION.md contracts |
| Mature | Continuous audit mode, drift detection, suggest governance updates |

## Cross-Validation (Constitution Ceremony)

During `at-init-setup`, after all discipline outputs are written into CONSTITUTION.md:

1. Read all discipline sections
2. Detect conflicts (e.g., "security says validate at controller" vs "architecture says validate at domain")
3. Surface conflicts to the user with trade-off analysis
4. Propose resolution or ask for decision
5. Only approve when conflicts are resolved
