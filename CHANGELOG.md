# Changelog

All notable changes to this project are documented here. Format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/); this project builds in
phases rather than semantic releases during v1 development.

## [Unreleased]

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
