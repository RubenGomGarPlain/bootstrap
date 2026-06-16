# Skill Design Standards (SOLID for skills)

Companion to `skill-quality-guidelines` (which covers *writing style*). This doc covers
*structure and responsibility* — the eight principles every skill and the catalog as a
whole must satisfy. Source: Matt Pocock's skills principles + the Claude Code plugin
reference.

## The eight principles

1. **Concise** — no filler; do not explain what the model already knows.
2. **Single responsibility** — one job per skill, not multi-step orchestration of unrelated concerns. Distinct modes that are really distinct responsibilities become separate skills or explicit entry points.
3. **Composable** — combinable with other skills; does not duplicate another skill's job.
4. **Progressively disclosed** — `SKILL.md` is a lightweight entry point; bulky templates, checklists, and examples live in `references/` loaded on demand.
5. **Harness-agnostic** — no hardcoding to a single platform or tool path.
6. **Well-documented** — third-person `description` + clear `Use when` / `Triggered by`.
7. **Portable** — no absolute paths or machine-specific assumptions; English throughout.
8. **Secure** — no leaked secrets; safe defaults.

## Commands are not a separate layer

The Claude Code plugin reference treats commands and skills as one primitive: a `commands/`
file is "a skill as a flat Markdown file," and `skills/` is recommended for new plugins. A
command that only delegates to a skill adds a second `/name` shortcut, double maintenance,
and double always-on tokens with zero new capability. **Do not keep a `commands/` layer that
duplicates skills.** Each skill's frontmatter is its own `/name` entry point.

## Per-skill compliance checklist

Run this against every `skills/*/SKILL.md`:

- [ ] **Concise** — no filler words; body earns its length
- [ ] **Single responsibility** — one job; no smuggled second workflow/mode
- [ ] **Composable** — does not re-implement another skill's job
- [ ] **Progressively disclosed** — large content in `references/`, linked from `SKILL.md`
- [ ] **Harness-agnostic** — no hardcoded platform/tool paths
- [ ] **Well-documented** — third-person `description`, `Use when`, `Triggered by`
- [ ] **Portable** — English only; no absolute paths
- [ ] **Secure** — no secrets; safe defaults
- [ ] **Not a stub** — has functional content
- [ ] **No command duplicate** — no `commands/` wrapper just pointing here
