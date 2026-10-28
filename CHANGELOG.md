# Changelog

All notable changes to this project are documented here. Format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/); this project builds in
phases rather than semantic releases during v1 development.

## [Unreleased]

### Repository

**Fixed**

- `.gitignore` `storage/` rule matched `src/Hirepath.Infrastructure/Storage/` on
  case-insensitive file systems, so two source files were never committed and a
  fresh clone failed to build. Anchored to `/storage/`.

**Added**

- MIT `LICENSE`; README status, screenshots, and highlights updated.

### Phase 1 — Data and intake

**Added**

- Application layer: `Result`/`AppError` types, repository/storage/extraction
  abstractions, a versioned prompt library, and the Settings, Profile, Resume,
  and JobDescription use-case services (shared by REST and, later, chat tools).
- `FakeChatClient` — a real `IChatClient` with a deterministic `DemoScript` —
  powering Demo Mode and every test; structured-output parsing of resumes and
  JDs against it.
- Infrastructure: EF repositories, PdfPig/OpenXml/plain-text extractors, and
  on-disk resume storage.
- API endpoints: settings, profile, resumes (multipart upload), and jobs, with
  typed `Result`→HTTP problem mapping and string-serialized enums.
- Web: Settings screen (typed reactive form, theme sync), Resumes screen
  (drag-and-drop upload, list, default, delete), and Job-descriptions screen
  (paste + parse). Shared component primitives, loading/empty/error states.

**Changed**

- Timestamps stored as sortable `long` so SQLite can order by them; initial
  migration regenerated. Startup migration skipped during OpenAPI doc emission.

**Notes**

- ADRs 0005 (SQLite ordering + DB-free doc gen) and 0006 (multipart upload via
  HttpClient). 33 tests green (backend 22, frontend 11); e2e 6.

### Phase 0 — Foundation

**Added**

- Solution scaffold: `Hirepath.Domain`, `.Application`, `.Infrastructure`,
  `.Api`, the Angular `Hirepath.Web`, and test projects, wired to the
  `Domain ← Application ← Infrastructure/Api` dependency rule.
- Domain model: Profile, Resume, JobDescription, MatchAnalysis, ChatThread/
  ChatMessage, and Settings entities with GUID v7 ids and UTC timestamps.
- EF Core 10 SQLite persistence, code-first, with the `InitialCreate` migration
  and JSON columns for complex properties. DB + storage rooted under
  `%LOCALAPPDATA%/Hirepath` with a config override.
- Minimal API with `AddOpenApi()`, CORS, startup migration, and health/app-info
  endpoints. Build-time OpenAPI emission feeds NSwag client generation.
- Angular 22 shell: zoneless, standalone, signals; Tailwind v4 + Material 22;
  an oklch design-token theme (dark default + light); the chat-first layout with
  an icon rail, conversation column, and canvas panel; onboarding empty state.
- Documentation set: `AGENTS.md`, `README.md`, `docs/{FEATURES,ROADMAP,
  ARCHITECTURE}.md`, ADRs 0001–0004, this changelog.
- Tooling: `.editorconfig`, `Directory.Build.props`, central package
  management, `run`/`verify` scripts, and a devcontainer.

**Notes**

- The app runs, looks good, and does nothing yet — by design for Phase 0.
- All verification gates green; no automated test touches the network.
