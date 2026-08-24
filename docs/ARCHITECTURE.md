# Architecture

Written for a human at 11pm, not for a compiler. If you only read one section,
read the "Where do I change X?" table.

## Project graph

```
Hirepath.Domain          entities, value objects, pure scoring logic — no deps
   ▲
Hirepath.Application      use cases, DTOs, IChatClient orchestration, tools, prompts
   ▲
Hirepath.Infrastructure   EF Core, file storage, AI providers, document parsing
   ▲
Hirepath.Api              Minimal API endpoints, SSE, OpenAPI, DI composition root
                          │  emits openapi.json at build
                          ▼
Hirepath.Web (Angular)    chat-first UI; NSwag generates its API client
```

**Dependency rule:** arrows point the only way references may go. `Domain`
references nothing. Anything that needs a database or a model lives in
`Infrastructure` or above — never in `Domain`.

## The request path for one analysis (end to end)

Target shape once Phases 2–3 land (Phase 0 wires the skeleton):

1. User asks in chat, or clicks **Run analysis** in the UI.
2. Chat: `IChatClient` (with `UseFunctionInvocation`) calls the
   `run_match_analysis` tool. UI: the same use-case handler is called via a REST
   endpoint. **One handler, two entry points.**
3. The handler loads the `Resume` and `JobDescription` from SQLite.
4. `Domain` computes the **deterministic sub-scores** (skill overlap, keyword
   coverage, years delta, …) — pure functions, no I/O.
5. A model pass (`GetResponseAsync<T>`) adds evidence quotes and narrative,
   schema-constrained into typed records.
6. The handler persists a new `MatchAnalysis` row (never overwrites) and emits a
   `match-summary` widget block with data straight from the database.
7. SSE streams progress and the result to the browser; Angular renders the
   widget inline and can open the full analysis in the canvas panel.

## How to add a chat tool

1. Write the use-case handler in `Hirepath.Application` (a plain method/record).
2. Expose it as an `AIFunction` via `AIFunctionFactory.Create(handler)` in the
   tool registry, **and** map the same handler to a REST endpoint in
   `Hirepath.Api`. Do not write the logic twice.
3. If it returns a widget, produce the widget payload server-side (see below).

## How to add a widget (three-step recipe)

1. **Define the payload type** — a typed interface for the widget's data.
2. **Write the component** — one Angular standalone component that renders it.
3. **Register it** — add `name → component` to the widget registry
   (`Record<string, Type<WidgetComponent>>`). An unknown name renders a graceful
   fallback card, never a crash. Bump the `version` when the shape changes so old
   persisted messages keep rendering.

Payloads are produced by server-side tool handlers, never by the model writing
JSON. The model chooses *which* widget and the prose around it; it can never
invent a score.

## Where do I change X?

| I want to… | Go to |
| :--- | :--- |
| Change the scoring weights or verdict thresholds | `appsettings.json` → `Hirepath:Ai`/scoring config (Phase 2); math in `Hirepath.Domain` |
| Change what a dimension score means | `Hirepath.Domain` scoring functions (pure, unit-tested) |
| Add / edit a domain entity | `Hirepath.Domain/<Area>/*.cs`, then a config in `Hirepath.Infrastructure/Persistence/Configurations/`, then add a migration |
| Add a DB column | edit the entity + its `IEntityTypeConfiguration`, then `dotnet-ef migrations add` |
| Add a chat tool | `Hirepath.Application` handler + tool registry + REST endpoint (see above) |
| Add / change a prompt | `Hirepath.Application/Prompts/*.md` (versioned) |
| Swap or add an AI provider | `Hirepath.Infrastructure` provider factory + settings; no domain change |
| Change the default model | `appsettings.json` → `Hirepath:Ai:Models`; overridable in Settings |
| Add an API endpoint | `Hirepath.Api/Endpoints/*.cs`, then `npm run generate:api` |
| Restyle the score ring / any colour | `src/Hirepath.Web/src/styles/tokens.css` (never hardcode a colour) |
| Change theme tokens (spacing, radius, type) | `src/Hirepath.Web/src/styles/tokens.css` |
| Add a widget | payload type + component + registry entry (three-step recipe above) |
| Change the shell layout / nav | `src/Hirepath.Web/src/app/core/layout/shell.*` |
| Add a screen / route | `src/Hirepath.Web/src/app/features/<feature>/` + `app.routes.ts` |
| Change where the DB / storage lives | `Hirepath.Infrastructure/Storage/AppPaths.cs` (or `Hirepath:DataRoot` config) |
| Regenerate the API client | `cd src/Hirepath.Web && npm run generate:api` |

## Data & storage locations

The SQLite file and the `storage/` folder live under
`%LOCALAPPDATA%/Hirepath/` (not the repo). Override with the `Hirepath:DataRoot`
configuration key or the `HIREPATH_DATA_ROOT` environment variable — tests and
the devcontainer use this to redirect to a throwaway folder.

## Frontend structure

`src/app/{core, shared, features/*}`. Standalone components, signals for state,
zoneless change detection, lazy routes, one `*-store.ts` signal store per
feature. The generated API client is under `core/api/` and is git-ignored.
