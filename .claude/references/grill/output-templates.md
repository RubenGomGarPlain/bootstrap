# Output templates (greenfield + brownfield)

## `CONTEXT.md`

Create or update at the project root, inside markers:

```markdown
<!-- AI-TEAM:START -->
## Stack
- Runtime: .NET 9
- Topology: <answer>
- Architecture: <answer>
- Auth: <answer>
- Frontend: <answer>

## Decisions
<one line per decision resolved in the debate>
<!-- AI-TEAM:END -->
```

## `docs/adr/`

Create one ADR per decision that meets all three criteria:
1. Hard to reverse
2. Surprising without context
3. The result of a real trade-off (alternatives existed)

Common candidates: architectural style, auth pattern, topology.
Format: `docs/adr/0001-<kebab-title>.md`

## `docs/domain-guides/<project-name>.md`

```markdown
# Domain Guide — <ProjectName>

## Architecture
- Style: <VSA+CQS | Clean Architecture | custom>
- Topology: <modular-monolith | microservices | single-service>

## Naming conventions
<detected or decided — classes, namespaces, files>

## Module structure
<folder pattern with an example>

## Test patterns
<framework, naming, location of test projects>

## Key decisions
<from ADRs — one line each>
```

## Handoff

```
✓ CONTEXT.md written
✓ docs/adr/000N-*.md written (N entries)
✓ docs/domain-guides/<project>.md written

Next step: run /at-plan to enter the plan→apply loop for the first story.
(Later, once a pattern repeats, /at-meta-improve forges a project-local skill.)
```
