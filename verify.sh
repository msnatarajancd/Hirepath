#!/usr/bin/env bash
# Runs every verification gate. Fails fast on the first red gate.
# Pass --skip-e2e to skip the Playwright flow (e.g. no browsers installed).
set -uo pipefail

root="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
web="$root/src/Hirepath.Web"
skip_e2e=0
[ "${1:-}" = "--skip-e2e" ] && skip_e2e=1

step() {
  echo ""
  echo "==> $1"
  shift
  if ! "$@"; then
    echo "FAILED: $*"
    exit 1
  fi
}

web_run() { (cd "$web" && "$@"); }

step 'Backend build (warnings as errors)' dotnet build "$root/Hirepath.slnx"
step 'Backend format check' dotnet format "$root/Hirepath.slnx" --verify-no-changes
step 'Backend tests' dotnet test "$root/Hirepath.slnx" --no-build

step 'Generate API client' web_run npm run generate:api
step 'Frontend lint' web_run npm run lint
step 'Frontend format check' web_run npm run format:check
step 'Frontend tests' web_run npm run test:ci
step 'Frontend production build' web_run npm run build

if [ "$skip_e2e" -eq 0 ]; then
  e2e_run() { (cd "$root/tests/Hirepath.E2E" && "$@"); }
  step 'End-to-end (Playwright, Demo Mode)' e2e_run npm test
fi

echo ""
echo "All gates passed."
