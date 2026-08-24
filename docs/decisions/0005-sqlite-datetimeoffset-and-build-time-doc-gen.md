# 0005 — SQLite DateTimeOffset ordering, and DB-free OpenAPI generation

**Status:** accepted · Phase 1

## Context

Two issues surfaced when the first real queries and the build-time OpenAPI
emission ran:

1. **SQLite cannot `ORDER BY` a `DateTimeOffset`** stored as TEXT — every
   `OrderByDescending(x => x.CreatedAt)` threw at runtime.
2. The build-time OpenAPI generator (`GetDocument.Insider`) boots the app to
   read the document, which ran the startup `MigrateAsync` against the real dev
   database — a build should never touch a database.

## Decision

1. Apply `DateTimeOffsetToBinaryConverter` globally in
   `HirepathDbContext.ConfigureConventions`, storing timestamps as a sortable
   `long`. All timestamps are UTC, so ordering is chronological. The initial
   migration was regenerated to reflect the `INTEGER` columns (no production
   data existed).
2. Guard the startup migration with `IsOpenApiDocumentGeneration()`, which
   checks whether the entry assembly is the `GetDocument` tool, and skips
   migration in that case. Tests (entry = test host) and real runs are
   unaffected.

## Consequences

- Ordering by `CreatedAt`/`UpdatedAt` works in SQL across all repositories.
- `dotnet build` emits the OpenAPI document without opening or migrating a
  database.
- If we later support non-UTC offsets, revisit the ordering assumption.
