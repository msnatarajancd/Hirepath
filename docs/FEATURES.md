# Features

The living ledger — what Hirepath does today and what's coming. Status values:
`shipped` · `in progress` · `planned` · `idea`.

| Feature | Area | Status | Version | Notes |
| :--- | :--- | :--- | :--- | :--- |
| App shell (chat-first layout, theme, nav rail) | Web | shipped | v1 | Phase 0 |
| Design-token theming (dark + light, oklch) | Web | shipped | v1 | Phase 0 |
| SQLite persistence + EF Core migrations | Data | shipped | v1 | Phase 0 |
| OpenAPI + generated Angular client | Platform | shipped | v1 | Phase 0 |
| Profile | Data | shipped | v1 | Phase 1 |
| Resume upload + text extraction (PDF/DOCX/TXT) | Intake | shipped | v1 | Phase 1 (PdfPig, OpenXml) |
| Job description paste + parse | Intake | shipped | v1 | Phase 1 |
| Settings screen (provider, model, key, theme) | Web | shipped | v1 | Phase 1 |
| FakeChatClient + Demo Mode | AI | shipped | v1 | Phase 1 |
| Deterministic scoring (Skills/Exp/Domain/Seniority/ATS) | Domain | planned | v1 | Phase 2 |
| Real providers (Anthropic / OpenAI / Ollama) | AI | planned | v1 | Phase 2 |
| Match analysis pipeline + SSE progress | Analysis | planned | v1 | Phase 2 |
| Full analysis canvas view | Web | planned | v1 | Phase 2 |
| AI Inspector (dev-only telemetry page) | AI | planned | v1 | Phase 2 |
| PII redaction toggle | AI | planned | v1 | Phase 2 |
| Conversational tool-calling assistant | AI | planned | v1 | Phase 3 |
| Widget protocol + renderer | Web | planned | v1 | Phase 3 |
| Follow-ups: explain gap, rewrite bullet, simulate, compare | Analysis | planned | v1 | Phase 3 |
| Thread persistence + pinned context | AI | planned | v1 | Phase 3 |
| Semantic skill matching (embeddings) | AI | planned | v1+ | Phase 2 stretch, behind a flag |

## Deliberately deferred (leave the seams, do not build in v1)

| Feature | Area | Status | Notes |
| :--- | :--- | :--- | :--- |
| Mock interviews (text) | Practice | planned | Chat tool + widget extension |
| Mock interviews (voice) | Practice | planned | `ISpeechToTextClient` seam |
| Coding tests with in-app execution | Practice | planned | |
| Application tracker | Organise | planned | |
| General career chat with live web context | AI | planned | |
| Desktop packaging (Electron/Photino) | Platform | planned | Keep web free of browser-only assumptions |
| Mobile packaging (Capacitor) | Platform | planned | |
| Gamification (streaks, XP, daily drills) | Engage | planned | |
| Resume A/B testing | Analysis | planned | |
| Job-board ingestion | Intake | planned | |
| Salary and offer analysis | Analysis | planned | |
| Referral tracking | Organise | planned | |
| Browser extension for one-click JD capture | Intake | planned | |
| Expose tool registry as an MCP server | Platform | planned | |
| Non-software career tracks | Scope | idea | |
