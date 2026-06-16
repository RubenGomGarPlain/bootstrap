---
name: skill-quality-guidelines
description: >
  Canonical writing standard for skills in this repository. Enforces caveman
  prose style, description format, body line limits, and reference depth rules.
  Use when creating or reviewing skill files.
  Triggered by: "write a skill", "create a skill", "review skill quality", "skill standards"
---

## Skill Quality Guidelines

Normative standard for all `SKILL.md` files in `skills/*/`.

---

## 1. Description Field (YAML frontmatter)

**Rules:**
- First sentence: capitalized, third-person action verb, ≤ 120 chars
- No "the team", "our", "my", or first-person pronouns
- Must include `Use when the user asks to:` list
- Must include `Triggered by:` phrase list

**Good:**
```
description: >
  Scaffolds .NET solution shell with Aspire and engineering defaults.
  Use when the user asks to: scaffold solution, new modular monolith.
  Triggered by: "new project", "scaffold solution", "bootstrap shell".
```

**Bad:**
```
description: >
  the team SPA frontend scaffolding for a solution...
```

---

## 2. Body Style (caveman prose)

All prose outside code blocks and tables: caveman style.

**Rules:**
- Drop articles (a/an/the), filler (just, simply, basically, feel free, you can)
- Fragments OK. Short synonyms. Technical terms exact.
- Pattern: `[thing] [action] [reason]`
- Code blocks, tables, command examples: unchanged — normal English

**Good:**
```
Generates `terraform/` at repo root with runnable config for chosen Azure target.
Provisions resources AppHost needs. Exposes stable `outputs.tf` contract.
```

**Bad:**
```
This skill generates a `terraform/` folder at the repo root with a complete,
runnable Terraform configuration for the chosen Azure target. The configuration
provisions exactly the resources the Aspire AppHost needs...
```

**Filler grep (must return 0 results):**
```bash
grep -rn "\bjust\b\|\bsimply\b\|\bbasically\b\|\byou can\b\|\bfeel free\b\|\bplease\b" skills/*/SKILL.md
```

---

## 3. Body Line Limit

**Limit:** ≤ 400 body lines (lines after second `---` frontmatter delimiter).

**Measure:**
```bash
awk 'BEGIN{c=0} /^---/{c++; next} c>=2{print}' SKILL.md | wc -l
```

Code blocks count toward the limit. If body exceeds 400 lines, move reference material to `references/` subdirectory and link.

---

## 4. Reference Depth

Cross-skill references: one hop only.

**Allowed:**
```markdown
[vertical-slice-cqs.md](../dotnet-guardrails/references/vertical-slice-cqs.md)
```

**Not allowed:**
```markdown
[something](../../other-skill/references/sub/nested.md)
```

No `../../` paths. No references to files outside `skills/`.

---

## 5. When-to-use Sections

**Do NOT include** a `## When to use` section with bullet lists of trigger phrases. That information belongs in the `description` frontmatter field as `Triggered by:` phrases. Duplicate sections inflate body line count without adding value.

---

## 6. Phase Headers

Use `## Phase N — Name` pattern for multi-phase skills. Each phase ends with **Checkpoint:** line summarizing what was verified.

---

## 7. Checklist (use when authoring or reviewing a skill)

- [ ] Description starts with capitalized third-person verb
- [ ] Description includes `Use when` and `Triggered by` lines
- [ ] No `## When to use` section in body
- [ ] Body prose uses caveman style — no filler words
- [ ] Body ≤ 400 lines
- [ ] All reference paths are one hop (`../other-skill/references/file.md`)
- [ ] Code blocks and tables left unchanged (no caveman inside code)
- [ ] Each phase has a **Checkpoint:** line
