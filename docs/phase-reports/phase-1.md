# Phase 1 — Data and intake — report

**Outcome:** a resume and a job description can be brought in, parsed, corrected,
and stored; the settings screen configures the provider; Demo Mode is explorable
without a key. All gates green.

## What shipped

**Application layer**
- `Result<T>` / `AppError` typed-error surface (with the AI error codes seeded
  for Phase 2). `UnsupportedDocumentException`.
- Abstractions: `I{Profile,Resume,JobDescription,Analysis,Settings}Repository`,
  `IResumeStorage`, `IDocumentTextExtractor(+Service)`.
- Versioned `PromptLibrary` loading embedded `Prompts/*.md` with named params
  and an id/version header.
- Services (one implementation, shared by REST and future chat tools):
  `SettingsService`, `ProfileService`, `ResumeService`, `JobDescriptionService`,
  plus `ResumeParser` / `JobDescriptionParser` using structured output.

**Infrastructure**
- `FakeChatClient` implementing `IChatClient` directly, driven by an
  `IFakeChatScript` (`DemoScript`) with invented fixtures. Registered via
  `AddChatClient`.
- EF repositories; PdfPig / OpenXml / plain-text extractors; `FileResumeStorage`.

**API**
- Endpoints: `/api/settings`, `/api/profile`, `/api/resumes` (multipart upload,
  list, get, set-default, delete, update-parsed), `/api/jobs`. `Result`→HTTP
  problem mapping. Enums serialized as strings. `IFormFile` schema transformer.

**Web**
- Settings screen: typed reactive form (provider segmented control, model, key,
  base URL, theme, Demo Mode, redact-PII), synced with the server; theme applied
  live. Loading/error states.
- Resumes screen: drag-and-drop + file-picker upload, list with default badge,
  set-default and delete, skeleton/empty/error states.
- Job-descriptions screen: paste + optional company/title, parse, list.
- Shared component primitives in `styles/components.css` (cards, inputs,
  segmented controls, switches, rows, dropzone, skeletons).

## Verification gates

| Gate | Result |
| :--- | :--- |
| `dotnet build` (warnings as errors) | ✅ |
| `dotnet format --verify-no-changes` | ✅ |
| `dotnet test` | ✅ Domain 2, Application 15, Api 5 |
| `npm run lint` | ✅ |
| `npm run test:ci` | ✅ 11 |
| `npm run build` (prod) | ✅ |
| Playwright (Demo Mode nav + shell) | ✅ 6 |
| Screenshots (3 widths × both themes) | ✅ reviewed |

## Decisions (ADRs)

- 0005 — SQLite can't order a `DateTimeOffset`; store as sortable `long`. Skip
  startup migration during build-time OpenAPI emission.
- 0006 — the one multipart upload posts via `HttpClient`; everything else uses
  the generated client (NSwag mis-generates the file upload).

## Could not verify / left for later

- **End-to-end intake through the browser+API together** is not yet in the
  Playwright run — booting the HTTPS API under Playwright needs dev-cert
  handling. The full intake path *is* covered by in-process API integration
  tests (`Resume_upload_parses_in_demo_mode`, unsupported-type → 415, settings
  round-trip). The browser e2e boots only the web server and asserts the screens
  render; API-backed e2e is planned for Phase 2/4.
- **Resume/JD correction UI** (editing the parsed result) — the API supports it
  (`update-parsed`); the inline editing UI is deferred to when the analysis view
  needs it (Phase 2/3).
- **PII redaction** is a stored setting only; the redacting middleware is Phase 2.

## For a human to look at

- `DemoScript` fixtures in `Infrastructure/Ai/Fake` — these shape the whole demo
  experience; tune the invented resume/JD to taste.
- Settings screen copy and the provider list.

## Next: Phase 2 — Real AI and analysis

Real providers behind `Microsoft.Extensions.AI` (with the middleware pipeline +
`PiiRedactionChatClient` + AI Inspector), deterministic scoring in `Domain` with
full unit tests, the analysis pipeline with SSE progress, and the full analysis
canvas view.
