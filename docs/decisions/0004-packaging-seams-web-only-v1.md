# 0004 — Desktop/mobile packaging deferred; keep the web app shell-able

**Status:** accepted · Phase 0

## Context

Desktop and mobile packaging are explicitly out of v1. But the spec requires the
Angular app to stay free of browser-only assumptions so an Electron/Photino
desktop shell and a Capacitor mobile shell can wrap it later without a rewrite.

## Decision

Record the intent now and hold to it as a constraint:

- No direct dependence on `window`-only globals scattered through features;
  browser APIs (e.g. `localStorage` in `ThemeStore`) stay behind small, easily
  replaceable services.
- The API base URL is injected (`API_BASE_URL`), not hardcoded, so a packaged
  build can point at a bundled or remote origin.
- All persistence is server-side over HTTP; the web layer holds no durable
  state a native shell would have to reimplement.

## Consequences

- v1 ships as a plain web app (API + Angular dev/prod server).
- A later desktop/mobile shell is an additive project, not a rewrite. Revisit
  with an ADR when that work starts.
