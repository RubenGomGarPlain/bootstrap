# Brownfield scan + validation

## Phase 1 — Scan

Detect from the existing repo (use shared signals in
[at-plan/references/project-detection.md](../../at-plan/references/project-detection.md)):
- Architecture style (VSA+CQS, Clean Architecture, custom)
- Naming conventions, folder structure, test patterns
- DI registration, middleware patterns
- Stack (.NET version, frontends, auth)
- Identifiable business modules

Present findings as a summary before continuing.

## Phase 2 — Validation interview (one question at a time)

1. **What are you building, and for whom?** — the scanner cannot detect the domain.
2. **Do the detected conventions fit?** — review the scan with the user; correct anything wrong.
3. **Pending decisions?** — same debate as the greenfield phase 3, adapted to what exists.
4. **Project name** — confirm or correct the detected value.
