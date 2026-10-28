# Roadmap

v1 is one feature — resume-to-role match, done conversationally — built on
foundations that make feature #2 cheap. Phases have explicit exit criteria; each
ends with all verification gates green, docs updated, and a phase report.

## Phase 0 — Foundation ✅

Solution, projects, docs, tooling, EF Core + initial migration, OpenAPI +
generated client, and the Angular shell rendering the conversation layout.

**Exit:** app runs, looks good, does nothing yet. All gates green.

## Phase 1 — Data and intake ✅

Profile, resume upload, document text extraction (PdfPig / OpenXml), JD paste,
persistence, settings screen, and `FakeChatClient` + Demo Mode. Resume/JD
parsing runs against the fake client with fixtures.

**Exit:** a resume and a JD can be brought in, parsed, corrected, and stored;
Demo Mode explorable without a key.

## Phase 2 — Real AI and analysis

Anthropic / OpenAI / Ollama clients behind `Microsoft.Extensions.AI`.
Deterministic scoring in `Domain` with full unit tests. The analysis pipeline
with SSE progress. The full analysis view in the canvas panel.

**Exit:** a real analysis streams in and is fully explainable; every number
traces back to a deterministic sub-score or a cited quote.

## Phase 3 — Conversation

Tool registry, function calling, streaming chat, the widget protocol and
renderer, pinned context, thread persistence, and every follow-up (explain gap,
rewrite bullet, simulate change, compare resumes).

**Exit:** the user can hold a genuinely useful conversation about the gap
between a resume and a role; reopening a thread re-renders it exactly.

## Phase 4 — Polish

Every screen at 375 / 768 / 1440 in both themes. Four async states, motion,
contrast, keyboard, real-provider error paths. Screenshots into the README.

**Exit:** ships. The "where do I change X?" table is accurate.

## Later, not now

Desktop/mobile packaging, mock interviews (text + voice), coding tests,
application tracker, gamification, resume A/B testing, job-board ingestion,
salary/offer analysis, referral tracking, browser extension, MCP server,
non-software tracks. See `FEATURES.md`. The chat tool registry, widget renderer,
and provider abstraction are the extension points that keep each one contained.
