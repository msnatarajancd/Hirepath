# Phase 0 — Foundation — report

**Outcome:** an app that runs, looks good, and does nothing yet. All
verification gates green.

## What shipped

**Repo & tooling**
- `git init` on `main`; `BUILD_PROMPT.md` committed first. `.gitignore`,
  `.gitattributes` (LF repo, CRLF for `*.ps1/cmd/bat`), `.editorconfig`,
  `Directory.Build.props` (nullable + warnings-as-errors), central package
  management (`Directory.Packages.props`).
- `run.ps1`/`run.sh`, `verify.ps1`/`verify.sh`, `.devcontainer/` (.NET 10 +
  Node 24), `dotnet-tools.json` (EF tools).

**Backend (.NET 10)**
- Solution (`.slnx`) with `Domain ← Application ← Infrastructure/Api` enforced by
  project references. Test projects for Domain, Application, Api.
- Domain model: Profile, Resume, JobDescription, MatchAnalysis (+ Dimension,
  Gap, ResumeSuggestion, PrepPlanItem, DeterministicScores), ChatThread/
  ChatMessage (+ ToolCallRecord, WidgetBlock), AppSettings. GUID v7 ids, UTC
  timestamps.
- EF Core 10 SQLite, code-first, JSON columns via a shared converter/comparer;
  `InitialCreate` migration. DB + storage under `%LOCALAPPDATA%/Hirepath` with a
  `Hirepath:DataRoot` / `HIREPATH_DATA_ROOT` override.
- Minimal API: `AddOpenApi()`, CORS, startup `MigrateAsync`, health/app-info
  endpoints via `TypedResults`. Build-time OpenAPI emission.

**Frontend (Angular 22)**
- Zoneless, standalone, signals; Tailwind v4 + Angular Material 22.
- oklch design-token system (`styles/tokens.css`) — dark default + complete
  light — wired into both Material and Tailwind. No component hardcodes a colour.
- Shell: icon rail (escape-hatch nav) + conversation column + canvas panel; one
  responsive decision (mobile drops the rail, canvas becomes a sheet).
- Chat front door with onboarding empty state, suggestion chips, working
  composer; honest placeholders for the other routes.
- NSwag generates the typed API client from the emitted OpenAPI doc
  (git-ignored, `npm run generate:api`).

**Docs**
- `AGENTS.md` (+ `CLAUDE.md` pointer), `README.md`, `docs/FEATURES.md`,
  `docs/ROADMAP.md`, `docs/ARCHITECTURE.md` (with the "where do I change X?"
  table), ADRs 0001–0004, `CHANGELOG.md`.

## Verification gates

| Gate | Result |
| :--- | :--- |
| `dotnet build` (warnings as errors) | ✅ 0/0 |
| `dotnet format --verify-no-changes` | ✅ |
| `dotnet test` | ✅ Domain 2, Api 2 |
| `npm run lint` | ✅ |
| `npm run test:ci` (Vitest + Testing Library) | ✅ 6 |
| `npm run build` (prod) | ✅ 77.7 kB transfer initial, chat lazy |
| Playwright (Demo Mode shell) | ✅ 3 |
| Screenshots (3 widths × both themes) | ✅ reviewed |

## Decisions (ADRs written)

- 0001 — xUnit v2; FluentAssertions pinned to 7.2.2 (last Apache-2.0 version;
  v8+ is commercially licensed).
- 0002 — Angular client generated from a build-time OpenAPI document.
- 0003 — Complex properties stored as JSON text columns.
- 0004 — Desktop/mobile packaging deferred; keep the web app shell-able.

Other notable choices: .NET 10 emits a `.slnx` solution (kept it); disabled
CA1716 (Resume vs. VB keyword) and CA1707 (underscored test names) as
inappropriate here; EF-generated migrations excluded from hand-written style
analyzers.

## Could not verify / left for later

- **Full Demo-Mode e2e flow** (upload → JD → analysis → follow-ups → reload) does
  not exist yet — nothing to exercise until Phase 1–3. The Playwright project and
  a shell smoke test are in place; the flow grows each phase. Phase 0 e2e boots
  only the web server (the onboarding screen makes no API call).
- **Mobile navigation:** the rail is hidden on mobile per the design; a mobile
  nav affordance (and the canvas sheet interactions) are a Phase 4 item.
- **Application.Tests** has no tests yet (no application code to cover); it will
  be populated in Phase 1.

## For a human to look at

- Palette/accent in `styles/tokens.css` — tune to taste; everything keys off it.
- The "where do I change X?" table in `docs/ARCHITECTURE.md` — sanity-check it
  matches your mental model before Phase 1 adds surface area.

## Next: Phase 1 — Data and intake

Profile, resume upload + PdfPig/OpenXml extraction, JD paste, persistence, the
settings screen, and `FakeChatClient` + Demo Mode with fixtures.
