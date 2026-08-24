# 0003 — Store complex properties as JSON text columns

**Status:** accepted · Phase 0

## Context

Several entities hold rich sub-structures: `ParsedResume`, `ParsedJd`, and the
analysis's `Dimensions`, `Gaps`, `ResumeSuggestions`, `PrepPlan`, and
`DeterministicScores`. These are read and written as whole graphs, rarely
queried field-by-field, and evolve in shape over time.

## Decision

Store them as **JSON text columns** using a small shared helper
(`Infrastructure/Persistence/JsonColumn.cs`) that provides an EF Core
`ValueConverter` (serialize/deserialize with `System.Text.Json`) plus a matching
`ValueComparer` so change-tracking works on the deserialized graph. Scalar
enums are stored as strings for readability.

## Consequences

- One boring, central place defines how our JSON columns behave.
- The schema stays small and stable; adding a field to a value record does not
  require a migration (though a migration is still added when the entity's own
  columns change).
- We are not indexing into these structures in SQL. If a future feature needs to
  query inside them, revisit with SQLite `json_extract` or a projection table.
