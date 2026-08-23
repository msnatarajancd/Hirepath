# Build Prompt — "Hirepath" v1

> **How to use this file.** Create an empty directory, save this file inside it as `BUILD_PROMPT.md`, open your coding agent of choice (Claude Code, Codex, Cursor, Kiro, Antigravity, Devin, Copilot Agent — any of them) and say:  
>   
> *"Read BUILD\_PROMPT.md and build v1 autonomously. Follow the build order, run the verification gates at every checkpoint, and write a phase report at each boundary. Do not ask me questions this document already answers."*

---

## 0\. Where this gets built, and first moves

**Target location:** `D:\Documents\Code Repos\hirepath` on Windows. Create that folder if it does not exist. Everything lives inside it; write nothing outside it.

**Before writing any code, verify the toolchain and stop with a clear message if anything is missing:**

dotnet \--list-sdks      \# need a .NET 10 SDK

node \--version          \# need Node 22.22+ (or 24.x) — Angular 22's minimum

npm \--version

git \--version

If a prerequisite is absent, do not attempt to install it silently or downgrade the stack. Print exactly what is missing, the download link, and stop.

**Git, from the first commit.** This repo will be pushed to a remote later, so treat history as public from the start:

1. `git init` in the project root, default branch `main`.  
2. Write `.gitignore` **before** the first `dotnet new` — a committed `bin/`, `obj/`, or `node_modules/` is painful to remove later. Cover: `bin/`, `obj/`, `node_modules/`, `dist/`, `.angular/`, `*.user`, `.vs/`, `.idea/`, `TestResults/`, `playwright-report/`, `*.db`, `*.db-shm`, `*.db-wal`, `storage/`, `appsettings.Local.json`, `.env*`, `docs/screenshots/*.tmp`.  
3. Write `.gitattributes`: `* text=auto eol=lf`, with `*.ps1`, `*.cmd`, `*.bat` forced to `crlf` and binaries marked. This repo must behave identically on Windows and Linux.  
4. Commit at every phase boundary with conventional-commit messages. Do not add a remote, do not push, do not create a GitHub repo — the user will do that.  
5. **Commit this file itself** (`BUILD_PROMPT.md`) in the first commit. The repo is the single source of truth for this project — spec, code, and docs live together and travel together. Nothing about this project lives in cloud storage.

**Secrets never enter the repo.** API keys are entered by the user in the app's settings screen and stored in the local SQLite database. For local development, `dotnet user-secrets` and environment variables are the only other accepted sources. No key, no matter how temporary, is written to any file under version control. Before the first commit and again in Phase 4, scan the working tree for anything resembling a key (`sk-`, `sk-ant-`, long base64 blobs) and fail loudly if found.

**Windows correctness matters here.** Use `Path.Combine` everywhere, never string concatenation with `/`. Put the SQLite file and the `storage/` folder under `Environment.GetFolderPath(SpecialFolder.LocalApplicationData)/Hirepath/` in normal runs — *not* inside the repo — with an override via configuration so tests and the devcontainer can redirect it. Provide both `run.ps1` and `run.sh`, and make sure `dotnet format` and Prettier agree about line endings so the tree is never dirty after a checkout.

---

## 1\. Mission

Build **Hirepath**, a local-first job-hunting companion.

**v1 does exactly one job, conversationally:** the user brings their resume and a job description, and Hirepath tells them how well they match, where the gaps are, and how to close them — then keeps talking about it. The analysis is not a report the user reads and closes. It is the start of a conversation: *"why did I score low on the cloud dimension?"*, *"rewrite my third bullet for this role"*, *"what if I finish the AZ-204 before applying?"*, *"compare this against my other resume."*

Everything runs on the user's own machine — a local API and a local SQLite file. No accounts, no cloud database, no deployment. Hosting and sync are out of scope for v1, but nothing in the architecture may make them hard later.

More features come in later versions. This build is one feature, done properly, on foundations that make feature \#2 cheap.

---

## 2\. The defining product decision: chat is the interface

**The primary way a user operates Hirepath is by typing or talking to it, not by navigating menus.** Traditional navigation exists, but as a fallback and an escape hatch — never as the main road.

This is not "add a chatbot in the corner." It means:

1. **The app opens to a conversation**, not a dashboard. A large, calm input with a prompt like *"Paste a job description, or drop your resume in."* and three or four suggestion chips.  
2. **The assistant can actually operate the app.** Every capability is exposed to the model as a callable tool: `upload_resume`, `save_job_description`, `run_match_analysis`, `explain_gap`, `rewrite_bullet`, `simulate_change`, `compare_resumes`, `list_analyses`, `open_analysis`. When the user says "analyse my resume against this JD," the model calls the tools; it does not reply with instructions telling the user which button to press.  
3. **Responses render as real UI, not walls of text.** The assistant's output stream carries typed *widgets* that Angular renders as live, interactive components inline in the conversation — a score ring, a gap list, a before/after bullet with a Copy button, a file drop zone, a resume picker. Prose explains; widgets do the work.  
4. **A canvas panel sits beside the conversation** for deep work. Clicking a widget opens the full analysis there. Chat drives on the left, detail lives on the right. On mobile the canvas becomes a full-screen sheet.  
5. **Everything the chat can do, a human can also do by hand.** Every tool has a plain UI path somewhere. The chat is the fast lane, not the only lane — an app you *must* talk to is a worse app than one you *can* talk to.

If a design choice would push the user toward hunting through a nav tree to find something, it is the wrong choice for this app.

---

## 3\. Non-negotiable constraints

Decisions already made. Do not re-litigate, do not substitute, do not ask.

| Area | Decision |
| :---- | :---- |
| Backend | ASP.NET Core on **.NET 10 (LTS)**, C\# 14, Minimal APIs, `nullable` \+ `TreatWarningsAsErrors` on |
| Frontend | **Angular 22** — standalone components only, **signals** for all state, **zoneless** change detection, new `@if`/`@for`/`@switch` control flow, typed reactive forms, `inject()` over constructor injection |
| UI kit | Angular Material 22 as the primitive layer, wrapped in our own themed components. Tailwind CSS v4 for layout and utilities |
| Database | **SQLite** via **EF Core 10**, code-first, real migrations from commit one. DB file under the app's local data folder |
| Files | Resume binaries on disk under a local `storage/` folder; DB stores the path \+ metadata |
| AI | **`Microsoft.Extensions.AI`** (`IChatClient`) as the abstraction. Providers: Anthropic, OpenAI, and Ollama — user picks in settings |
| Realtime | Server-Sent Events for streaming chat and analysis progress |
| API contract | OpenAPI generated by .NET's built-in `AddOpenApi()`; the Angular client **generated** from it with **NSwag** into `src/app/core/api/` (git-ignored, regenerated by an npm script), never hand-written |
| Ports | API on `https://localhost:7180`, Angular dev server on `http://localhost:4200` with a proxy to the API. Both configurable |
| Tests | xUnit \+ FluentAssertions \+ Testcontainers-free in-memory/SQLite tests (backend); Vitest \+ Angular Testing Library (frontend); Playwright (e2e) |
| Tooling | `dotnet` CLI \+ Angular CLI \+ npm. No proprietary build system, no IDE-specific project files required |

**Versions:** use the latest stable release in each of the above lines at build time. If a newer stable minor exists, take it, and note it in the ADR log. Do not use previews.

---

## 4\. Tool-agnostic, human-editable — a hard requirement

The user is a .NET/Angular developer who will hand agents most of the work but must be able to open this repo in **any** editor or AI tool — VS Code, Visual Studio, Rider, Neovim, Cursor, Kiro, Antigravity, Devin, Codex — and immediately read, run, and fix it.

Therefore:

- **`AGENTS.md` at the repo root is the single agent instruction file.** It is the cross-tool standard and is read by most coding agents. Put the real content there: commands, conventions, architecture rules, definition of done. Create `CLAUDE.md` as a one-line pointer (`See AGENTS.md`) rather than duplicating content that will drift.  
- **Everything is plain text in the repo.** No settings that live only in one IDE, no generated code committed without its generator, no hidden state.  
- **`.editorconfig`** at the root governing C\# and TypeScript style, so any editor formats identically. `dotnet format` and Prettier/ESLint enforce it in CI.  
- **One command to run everything.** `dotnet run --project src/Hirepath.AppHost` (or a documented `./run.sh` / `run.ps1` doing the same on Windows, macOS, Linux) starts API \+ Angular dev server. Document the manual two-terminal path too, for anyone who prefers it.  
- **A `.devcontainer/`** so the whole thing boots in a container with zero local installs.  
- **`docs/ARCHITECTURE.md` must contain a "where do I change X?" table** — a literal map from common tasks ("change the scoring weights", "add a new chat tool", "add a DB column", "restyle the score ring") to the exact files. Written for a human at 11pm, not for an agent.  
- Standard, boring, widely-documented patterns everywhere. Clever code is a tax on both the human and the next agent.

---

## 5\. Solution layout

hirepath/

├─ AGENTS.md                      \# the agent operating manual (CLAUDE.md points here)

├─ README.md  CHANGELOG.md  .editorconfig  Directory.Build.props

├─ Hirepath.sln

├─ run.sh / run.ps1

├─ .devcontainer/

├─ docs/

│  ├─ FEATURES.md                 \# living feature ledger

│  ├─ ROADMAP.md

│  ├─ ARCHITECTURE.md             \# includes the "where do I change X?" table

│  └─ decisions/0001-\*.md         \# ADRs

├─ src/

│  ├─ Hirepath.Domain/            \# entities, value objects, pure scoring logic. No dependencies.

│  ├─ Hirepath.Application/       \# use cases, DTOs, the IChatClient orchestration, tool definitions

│  ├─ Hirepath.Infrastructure/    \# EF Core, file storage, AI providers, document parsing

│  ├─ Hirepath.Api/               \# Minimal API endpoints, SSE, OpenAPI, DI composition root

│  └─ Hirepath.Web/               \# Angular 22 app

└─ tests/

   ├─ Hirepath.Domain.Tests/

   ├─ Hirepath.Application.Tests/

   ├─ Hirepath.Api.Tests/         \# WebApplicationFactory integration tests

   └─ Hirepath.E2E/               \# Playwright

Dependency rule: `Domain` ← `Application` ← `Infrastructure` / `Api`. `Domain` references nothing. Scoring math, keyword coverage, and experience-gap calculations live in `Domain` as pure functions so they are trivially unit-testable without a database or a model.

Angular structure: `src/app/{core,shared,features/chat,features/analysis,features/resume,features/settings}`, standalone components, lazy routes, one `*.store.ts` signal store per feature.

---

## 6\. Domain model (v1)

EF Core entities, all with `Id` (GUID v7), `CreatedAt`, `UpdatedAt`.

- **Profile** (single row) — name, email, location, years of experience, target roles, seniority, links.  
- **Resume** — label, file name, content type, stored path, extracted text, `ParsedResume` (JSON column): summary, skills, experience (company/title/dates/bullets), education, projects, certifications. `IsDefault`.  
- **JobDescription** — company, title, location, work mode, seniority, raw text, `ParsedJd` (JSON): responsibilities, required skills, preferred skills, years required, tech stack, keywords.  
- **MatchAnalysis** — resume id, JD id, `OverallScore` 0–100, `Verdict` (`Strong | Good | Stretch | Poor`), `Dimensions` (name, score, weight, evidence quotes, rationale) across **Skills, Experience, Domain, Seniority, ATS Keywords**, `Strengths`, `Gaps` (item, severity, why it matters *to this employer*, how to close it, estimated effort), `ResumeSuggestions` (target bullet, suggested rewrite, reason), `PrepPlan` (a 7-day checklist with persisted checked state), plus the raw deterministic sub-scores.  
- **ChatThread / ChatMessage** — thread with pinned context (resume id, JD id, analysis id). Messages carry role, text, tool calls with arguments and results, and rendered widgets — so a reopened thread looks exactly as it did.  
- **Settings** — provider, model, API key, base URL, theme.

Scores must be **explainable**. Keyword coverage, years-of-experience delta, and skill overlap are computed deterministically in `Domain` with unit tests. The model supplies semantic judgment, evidence extraction, and narrative. Store both, so any number can be traced back. Re-running an analysis creates a new record; nothing is overwritten.

---

## 7\. The AI layer

- Build on `Microsoft.Extensions.AI`. Register an `IChatClient` per configured provider behind a factory; add the function-invocation and logging middleware from the same package. Swapping Anthropic → OpenAI → Ollama must be a settings change, not a code change.  
- **Provider packages:** OpenAI has a first-party `Microsoft.Extensions.AI.OpenAI` adapter — use it, and reach Ollama through its OpenAI-compatible endpoint with the same adapter and a different base URL. For Anthropic, check NuGet at build time for a maintained package exposing an `IChatClient` (`Anthropic.SDK` and the official `Anthropic` package are the candidates); if the one you pick does not cleanly expose `IChatClient`, write a thin adapter over the HTTP API yourself rather than contorting the abstraction. Record the choice as an ADR.  
- **Model IDs are configuration, never constants in code.** Ship sensible defaults in `appsettings.json`, let the settings screen override them, and do not hardcode a model name anywhere in the domain or application layer.  
- **A `FakeChatClient` comes first**, before any real provider. It returns deterministic scripted responses and tool calls. **Every automated test uses it** — no test touches the network. It also powers **Demo Mode**, a toggle that lets the whole app be explored without an API key. Demo Mode is required for v1: it is how the UI gets reviewed and how e2e tests run.  
- **Tools** are declared once in `Application` as `AIFunction`s over the same use-case handlers the REST endpoints call. One implementation, two entry points — chat and HTTP. Never two copies.  
- **Structured output** for parsing and analysis: JSON-schema-constrained responses deserialised into typed records and validated. On a validation failure, retry once with the error appended, then surface a typed, recoverable error. Never regex a JSON blob out of prose.  
- **Prompts live as versioned files** under `Hirepath.Application/Prompts/*.md`, loaded and rendered with named parameters. Each has an id and a version. No prompt strings inline at call sites.  
- **API keys** live in the local settings store, never in source, never logged, never sent anywhere but the chosen provider. Say so plainly on the settings screen. Support environment-variable override for developers.  
- **Typed error surface** — `RateLimited | Unauthorized | Timeout | InvalidResponse | ProviderDown` — each rendered by the UI as a recoverable state with a retry, never a blank screen or a stack trace.  
- Streaming: token-by-token to the browser over SSE, with tool-call activity shown as it happens ("Reading your resume…", "Scoring against the JD…") — the wait is part of the experience, not a spinner.

### 7.1 The `Microsoft.Extensions.AI` pipeline — build it properly

This library is a first-class part of the architecture, and the build should exercise it deliberately rather than treating `IChatClient` as a thin HTTP wrapper. Compose the client as a **middleware pipeline**, in this order, and put the composition in one readable file in the API's DI setup:

builder.Services.AddChatClient(sp \=\> CreateProviderClient(sp))

    .UseLogging()             // structured logs of every request/response

    .UseDistributedCache()    // skip identical calls

    .UseFunctionInvocation()  // the tool loop that makes chat-as-interface work

    .UseOpenTelemetry();      // token counts, latency, spans

Each layer earns its place in this app:

- **`UseFunctionInvocation()`** runs the tool-calling loop automatically. Declare tools with `AIFunctionFactory.Create(handler)` over the existing use-case handlers — this is what lets the assistant *operate* the app rather than describe it. Without it you would hand-roll the loop.  
- **`UseDistributedCache()`** backed by SQLite. Re-parsing the same resume or re-running an identical analysis should cost nothing. Cache keys must include the model id and prompt version, so bumping a prompt invalidates cleanly. Never cache a streaming conversation turn.  
- **`UseOpenTelemetry()`** feeds a **dev-only "AI Inspector" page** in the app: every call with its model, token counts in and out, latency, estimated cost, and cache hit or miss. This is genuinely useful in a local app where the user pays per token, and it is the fastest way to understand what the abstraction is actually doing. Build it — it is small.  
- **One hand-written middleware.** Implement a `DelegatingChatClient` subclass of your own — `PiiRedactionChatClient` is the right one here: resumes contain phone numbers, addresses, and sometimes visa status, and the user should be able to toggle "redact contact details before sending to the model." Writing one decorator teaches the whole design of the library from the inside, and this one is a real feature rather than a toy.

**Typed structured output** uses `GetResponseAsync<T>(...)` rather than manual JSON handling — resume parsing, JD parsing, and the analysis result all come back as typed records with schema-constrained generation. This is the single biggest reliability win the library offers; use it everywhere a shape is expected.

**`FakeChatClient` implements `IChatClient` directly** — not a mock framework, a real implementation. That is deliberate: it is the clearest way to learn the interface's contract, including the streaming and function-calling paths.

**Optional stretch in Phase 2, behind a config flag, with a clean fallback:** add `IEmbeddingGenerator<string, Embedding<float>>` and **`Microsoft.Extensions.VectorData`** over a SQLite vector store for **semantic skill matching** — so a JD asking for "ORM experience" matches a resume saying "EF Core," which pure keyword overlap misses. If embeddings are unavailable or the flag is off, fall back to the deterministic keyword path with no loss of function. Record it as an ADR.

**Deliberately deferred to later versions, but leave the seams:** `ISpeechToTextClient` for voice mock interviews, `IImageGenerator` (no use here yet), and exposing Hirepath's tool registry as an **MCP server** so other agents could drive it. Note all three in `docs/FEATURES.md` as `planned`.

---

## 8\. The one feature, in detail

**Intake.** Resume upload by drag-and-drop anywhere in the app, or by asking the assistant. PDF, DOCX, TXT, max 10 MB. Parse server-side — PDF text via **PdfPig** (Apache 2.0, actively maintained, no native dependencies), DOCX via **DocumentFormat.OpenXml** — then a model pass into `ParsedResume`. JD by paste, with a model pass into `ParsedJd`. Both must handle messy real-world input without crashing; show what was extracted and let the user correct it.

**Analysis.** Deterministic sub-scores first, then the model pass for evidence and narrative, streamed so results appear progressively rather than after a long blank wait.

**Presentation.** The result arrives in the conversation as a compact card — score ring, verdict, the top three gaps — with an "Open full analysis" affordance that fills the canvas panel. The full analysis contains:

- The animated score ring and verdict.  
- Dimension bars, each expandable to the exact resume quotes that justify the score.  
- The gap list, sorted by severity, each gap with why it matters to *this* employer, a concrete closing action, and an effort estimate.  
- An ATS keyword panel — JD keywords present vs. missing, click to see where they appear.  
- Resume suggestions as before/after bullets with one-click copy.  
- The 7-day prep plan as a persisted checklist.

**The conversation is the point.** With an analysis pinned as context, the user can ask anything and the assistant answers against the real stored data:

- *"Why is my seniority score only 60?"* → cites the specific evidence behind that dimension.  
- *"Rewrite bullet 3 for this role."* → `rewrite_bullet` tool, returns a before/after widget.  
- *"What if I add Kubernetes and Terraform?"* → `simulate_change` recomputes the deterministic sub-scores against a hypothetical and shows the delta. It must be honest that this is a simulation, not a claim about the real resume.  
- *"Which of my two resumes fits better?"* → `compare_resumes`, side-by-side widget.  
- *"Give me the 30-second version I'd say in a screening call."*

Follow-ups must be **fast and cheap** — reuse the stored analysis as context rather than re-running the whole pipeline, and make that obvious in the code.

**Deliberately out of v1:** mock interviews, coding tests, the application tracker, voice, web search, gamification. They go in `docs/FEATURES.md` as `planned` on day one. Do not build them. Do leave the seams: the chat tool registry, the widget renderer, and the provider abstraction are the extension points that make each of them a contained addition.

---

## 9\. Design bar

The UI is a requirement, not decoration. Target feeling: **a calm, precise thinking tool** — Linear's restraint, a conversation's warmth. If a screen looks like default Angular Material, it is not done.

- **Tokens, not values.** All colour, spacing, radius, shadow, and type live as CSS custom properties in one theme file, wired into both the Material theme and the Tailwind config. Dark and light are both complete; dark is default. No component hardcodes a colour.  
- **Palette:** near-black neutral base defined in `oklch`, one confident accent, and a semantic ramp for success/warning/danger/info. One `scoreToColor` function used everywhere, so 72 always looks like 72\.  
- **Type:** one variable sans for UI (Inter or Geist), one mono for code and metrics. A fixed scale; no ad-hoc sizes.  
- **Layout:** conversation column plus canvas panel on desktop, with a slim collapsible icon rail for the escape-hatch navigation. On mobile: full-width conversation, canvas as a sheet, no rail. One responsive decision in the shell, not media queries scattered per component.  
- **Motion:** Angular animations, restrained. A spring on the score ring, 120–200ms transitions, staggered list entrance capped around 40ms, streaming text that doesn't jitter the scroll position. Honour `prefers-reduced-motion` everywhere.  
- **Four states designed for every async surface** — loading (skeletons shaped like the real content, never a bare spinner), empty (icon, one sentence, one action), error (what happened, what to do, retry), success.  
- **The empty first-run state carries the onboarding.** A new user sees an inviting prompt and suggestion chips, not an empty table.  
- **Keyboard-first:** `⌘K` / `Ctrl+K` opens the command input from anywhere, `?` shows shortcuts, correct tab order throughout.  
- **Accessibility is a gate:** WCAG AA contrast in both themes, visible focus rings, ARIA on custom widgets, ≥44px touch targets, screen-reader text for the score ring and every chart.

---

## 10\. Build order

Work in phases. At the end of each: run every gate in §11, update the docs in §12, commit, and write a report to `docs/phase-reports/phase-N.md` — what shipped, what you decided and why, what you could not verify, and anything you want a human to look at. Also print it. Then continue — nobody may be watching, so do not block on approval unless a gate fails twice or you hit a genuine fork this document does not resolve. If you must stop, say precisely what decision you need.

**Phase 0 — Foundation.** Solution, projects, `AGENTS.md`, all docs, `.editorconfig`, devcontainer, run scripts, EF Core with the initial migration, OpenAPI \+ generated Angular client, the Angular shell with theme tokens, and the conversation layout rendering static placeholder messages. Ships an app that runs, looks good, and does nothing yet.

**Phase 1 — Data and intake.** Profile, resume upload, document text extraction, JD paste, the persistence layer, the settings screen, and `FakeChatClient` \+ Demo Mode. Resume and JD parsing runs against the fake client with fixtures.

**Phase 2 — Real AI and analysis.** Anthropic, OpenAI, and Ollama clients. Deterministic scoring in `Domain` with full unit tests. The analysis pipeline with SSE progress. The full analysis view in the canvas panel.

**Phase 3 — Conversation.** The tool registry, function calling, streaming chat, the widget protocol and renderer, pinned context, thread persistence, and every follow-up capability in §8. This is the phase that makes the product what it is — give it room.

**Phase 4 — Polish.** Every screen at 375 / 768 / 1440, both themes. State coverage, motion, contrast, keyboard traps, error paths with a real provider failing. Screenshots into the README. Then a final pass on `docs/ARCHITECTURE.md` so the "where do I change X?" table is actually accurate.

Desktop and mobile packaging are **not** in v1. Keep the Angular app free of browser-only assumptions so an Electron/Photino desktop shell and a Capacitor mobile shell can wrap it later without a rewrite. Record that intent as an ADR.

---

## 11\. Verification gates

Make these one command (`./verify.sh` and `verify.ps1`) and pass them at every checkpoint:

1. `dotnet build` — zero warnings, warnings-as-errors on.  
2. `dotnet format --verify-no-changes` and `npm run lint` — clean.  
3. `dotnet test` — every pure function in `Domain` covered (scoring, keyword coverage, experience delta, verdict thresholds), every repository against SQLite, every endpoint via `WebApplicationFactory`. No test hits the network.  
4. `npm test` — signal stores, the widget renderer, and every component with meaningful logic.  
5. `npx playwright test` — in Demo Mode: first run → upload a fixture resume → paste a fixture JD → analysis streams in → open the full analysis → ask two follow-up questions and get widget responses → reload and find the thread intact. This flow stays green forever.  
6. `npm run build` — production build succeeds; report bundle size and lazy-load anything heavy.  
7. **Look at it.** Playwright screenshots of every screen at three widths in both themes into `docs/screenshots/`, and actually review them before calling a phase done.

**Definition of done for any feature:** works with a real provider, works in Demo Mode, has all four async states, is keyboard-navigable, passes every gate, and is marked `shipped` in `docs/FEATURES.md`.

---

## 12\. Documentation discipline

Create these in Phase 0; a phase is not complete until they are updated in the same commit.

- **`README.md`** — what Hirepath is, screenshots, prerequisites, one-command start, how to configure a provider, how to run Demo Mode, the scripts table, the project map, current status.  
- **`docs/FEATURES.md`** — the living ledger, and the answer to "what does this app do and what's coming." A table: Feature | Area | Status (`shipped` / `in progress` / `planned` / `idea`) | Version | Notes. Seed it with the v1 feature and every deferred one: mock interviews (text and voice), coding tests with in-app execution, application tracker, general career chat with live web context, desktop and mobile packaging, gamification (streaks, XP, daily drills), resume A/B testing, job-board ingestion, salary and offer analysis, referral tracking, a browser extension for one-click JD capture, non-software career tracks.  
- **`docs/ROADMAP.md`** — phases with exit criteria, plus a "later, not now" section.  
- **`docs/ARCHITECTURE.md`** — project graph, dependency rule, the request path for one analysis end to end, how to add a chat tool, how to add a widget, and the "where do I change X?" table.  
- **`docs/decisions/NNNN-title.md`** — one short ADR per non-obvious choice: context, decision, consequences. Write one whenever you extend or deviate from this document.  
- **`CHANGELOG.md`** — Keep a Changelog format, per phase.  
- **`AGENTS.md`** — for the next agent: commands, conventions, the dependency rule, "prompts live in `Application/Prompts`", "tools are declared once and shared with the REST layer", "tests use `FakeChatClient` only", the doc-update requirement, and the definition of done.

---

## 13\. Operating rules

- **Do not ask questions this document answers.** Where it is silent, choose what a thoughtful senior engineer would choose, implement it, and record an ADR.  
- **Small, focused commits**, conventional-commit messages, at least one per phase boundary.  
- **Write tests alongside the code**, not after the phase.  
- **Never fake it.** No hardcoded sample scores presented as real output, no stub behind a finished-looking UI, no swallowed exceptions. If something isn't built, `FEATURES.md` says `planned` and the UI says so honestly.  
- **Boring and readable beats clever.** A .NET/Angular developer must be able to open any file and understand it in a minute.  
- **Keep files small** — roughly 300 lines. Organise by feature, not by file type.  
- **Two failed attempts at the same problem means stop**, write down what you tried in the phase report, and move to the next independent piece rather than thrashing.

---

## 14\. Deliverable

A running application. On a clean machine, one documented command starts it, and the user lands in a conversation. In Demo Mode they can explore the entire product. With an API key pasted in, they can drop in a real resume and a real job description and have a genuinely useful conversation about the gap between them. Plus the documentation set in §12, kept honest.

Begin with Phase 0\.

---

## Appendix A — The widget protocol

This is the novel part of the build and the place most likely to go wrong, so it is specified rather than left to judgment.

An assistant turn is a sequence of **blocks** streamed to the client. Two kinds:

{ "type": "text",   "text": "Your strongest match is on the .NET side…" }

{ "type": "widget", "widget": "match-summary", "version": 1, "payload": { /\* typed \*/ } }

Rules:

- Widget payloads are **produced by server-side tool handlers**, never by the model writing JSON. The model calls `run_match_analysis`; the handler runs the real pipeline and emits the widget with data straight from the database. The model can decide *which* widget to show and what prose to wrap around it. It can never invent a score.  
- Every widget name maps to exactly one Angular standalone component, resolved through a registry (`Record<string, Type<WidgetComponent>>`) with a typed payload interface per widget. An unknown widget name renders a graceful fallback card, never a crash.  
- The v1 widget set: `match-summary`, `gap-list`, `dimension-breakdown`, `bullet-rewrite`, `keyword-coverage`, `prep-plan`, `resume-picker`, `file-drop`, `resume-comparison`, `simulation-delta`, `confirm-action`.  
- Widgets are **interactive**: a button inside one sends a new turn back into the conversation (`resume-picker` → picks a resume; `confirm-action` → runs the pending tool). Widget state that matters (checked prep-plan items) persists to the database, not just to component state.  
- Widgets are **persisted with the message** so reopening a thread re-renders it exactly. Include the version so old messages keep rendering after a widget changes shape.  
- Adding a widget must be a three-step recipe documented in `ARCHITECTURE.md`: define the payload type, write the component, register it.

## Appendix B — Scoring defaults

Put these in configuration, not in code, so they can be tuned without a rebuild. Defaults:

| Dimension | Weight | Deterministic input | Model input |
| :---- | :---- | :---- | :---- |
| Skills | 0.30 | overlap of JD required+preferred skills with resume skills | judges equivalence (e.g. "EF Core" satisfies "ORM experience") |
| Experience | 0.25 | years delta vs. JD requirement, recency of relevant roles | judges depth and relevance of the actual work |
| Domain | 0.15 | industry/keyword overlap | judges transferability |
| Seniority | 0.15 | title ladder mapping, scope signals | judges leadership and ownership evidence |
| ATS keywords | 0.15 | exact and stemmed keyword coverage | none — purely mechanical |

Verdict thresholds: `Strong ≥ 80`, `Good 65–79`, `Stretch 45–64`, `Poor < 45`. Every dimension score must carry at least one evidence quote from the resume or an explicit "no evidence found" — a score without justification is a bug.

## Appendix C — Test fixtures

Create synthetic fixtures under `tests/fixtures/` — a plausible mid-level .NET/Angular resume as PDF, DOCX, and TXT, plus two job descriptions (one strong match, one stretch). **Invent them entirely.** No real person's resume, no real company's posting, no personal data of any kind in the repo. The `FakeChatClient` scripts its responses against these fixtures so the e2e flow is fully deterministic.  
