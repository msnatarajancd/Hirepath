# 0001 — Test stack, and pinning FluentAssertions to 7.x

**Status:** accepted · Phase 0

## Context

The build spec mandates xUnit + FluentAssertions. Two details need a decision:

- xUnit ships as a stable v2 line and a newer v3 line.
- FluentAssertions **v8+ is licensed commercially** (Xceed); v7.x is the last
  release under the free Apache-2.0 license.

## Decision

- Use **xUnit v2 (2.9.3)** with `xunit.runner.visualstudio` 3.x. It is the most
  widely documented combination and matches the SDK's `dotnet new xunit`
  template output — "boring and readable".
- Pin **FluentAssertions to 7.2.2**, the last Apache-2.0 version, via central
  package management. This keeps the repo free of a commercial license
  obligation for a local, open project.

## Consequences

- No paid-license surprise for anyone who clones and runs the tests.
- If we ever want FluentAssertions v8 features, that is a deliberate,
  license-aware upgrade — not something that happens silently on a version bump.
- Central Package Management (`Directory.Packages.props`) makes the pin explicit
  and single-sourced.
