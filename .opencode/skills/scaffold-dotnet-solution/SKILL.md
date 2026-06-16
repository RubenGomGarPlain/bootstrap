---
name: scaffold-dotnet-solution
user-invocable: false
description: >
  Scaffolds a .NET solution structure from a confirmed bootstrap interview, driven by the shell and arch domain guides.
  Model-invocable building block — orchestrated by commands (e.g. /at-init-bootstrap), not a user entry point.
  Invoked by a command when it needs to: scaffold a .NET solution, create the project structure, lay down architecture layers.
---

> Turns confirmed interview answers into a building solution. Caller supplies the answers + the domain guides; this skill lays down files and verifies the build.

## Input
- Confirmed bootstrap answers: stack, architectural style, internal organization, project name.
- `shell` and `arch` domain guides (passed by the caller — this skill does not reach shared references itself).

## Phase 1 — Verify clean ground

**Why:** scaffolding over an existing solution corrupts it.

Confirm no conflicting solution exists (caller passes a `detect-architecture` result, or this skill checks for `*.sln`/`*.slnx`).

**Checkpoint:** no existing solution conflict.

## Phase 2 — Scaffold structure (shell)

**Why:** the shell guide defines the entry-point topology and folder layout the team chose.

Lay down solution + project files per the `shell` guide and the confirmed organization (vsa-cqs / clean-arch / minimal).

**Checkpoint:** solution restores; folders match the chosen style.

## Phase 3 — Apply architecture layers (arch)

**Why:** layer boundaries are load-bearing decisions; applying them now avoids retrofits.

Create the architecture layers per the `arch` guide. Verify dependency direction.

**Checkpoint:** `dotnet build` passes; arch tests pass if present.

## Constraints
- Scaffolds files only; does not author ADRs (caller invokes `write-adr`) and does not write CONTEXT.md (caller invokes `write-context`).
- Return the created paths + build result to the caller; do not invoke other skills.
