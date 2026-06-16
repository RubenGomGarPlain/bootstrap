# Context-map format (vertical, per-module context)

Three distinct artifacts, split by path and purpose. Do not conflate them.

| Artifact | Path | Purpose |
|----------|------|---------|
| Project-state | `CONTEXT.md` (root) | stack / architecture / hooks / CodeGraph / ADR list |
| Context map | `CONTEXT-MAP.md` (root) | index of bounded contexts + their relationships |
| Module glossary | `src/<module>/CONTEXT.md` | that module's ubiquitous language |

`CONTEXT.md` at the **root** = project-state. `CONTEXT.md` **inside a module** = that module's glossary. The map is the disambiguator and the entry point to the domain layer.

## Module glossary format (`src/<module>/CONTEXT.md`)

Glossary only — ubiquitous language, no implementation detail, no spec, no scratchpad.

```md
# {Context Name}

{One or two sentences: what this context is and why it exists.}

## Language

**Order**:
A customer's request to buy goods, before payment is taken.
_Avoid_: Purchase, transaction

**Invoice**:
A request for payment sent after delivery.
_Avoid_: Bill, payment request
```

- Opinionated: pick one word; list synonyms under `_Avoid_`.
- Definitions tight (1–2 sentences). Define what it IS, not what it does.
- Only terms specific to this context — not general programming concepts.
- Group under subheadings when natural clusters emerge.

## Context map format (`CONTEXT-MAP.md`)

```md
# Context Map

## Contexts

- [Ordering](./src/ordering/CONTEXT.md) — receives and tracks customer orders
- [Billing](./src/billing/CONTEXT.md) — generates invoices and processes payments
- [Fulfillment](./src/fulfillment/CONTEXT.md) — warehouse picking and shipping

## Relationships

- **Ordering → Fulfillment**: Ordering emits `OrderPlaced`; Fulfillment consumes it to start picking
- **Fulfillment → Billing**: Fulfillment emits `ShipmentDispatched`; Billing generates the invoice
- **Ordering ↔ Billing**: shared types `CustomerId`, `Money`
```

- Locations are explicit paths — a non-`src/` layout is supported.
- `## Relationships` is what makes loading vertical: it tells the loader which neighbour a cross-boundary change pulls in.

## Detection rules

- `CONTEXT-MAP.md` exists → **multi-context** (read it for the module list).
- No map, root `CONTEXT.md` exists → **single-context** (no domain layer; today's behaviour).
- Neither → **none** (domain layer not started; created lazily by a later authoring step).
