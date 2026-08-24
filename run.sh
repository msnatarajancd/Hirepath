#!/usr/bin/env bash
# Starts Hirepath: the API (https://localhost:7180) and the Angular dev server
# (http://localhost:4200, proxied to the API). Ctrl+C stops both.
set -euo pipefail

root="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
web="$root/src/Hirepath.Web"

echo "==> Building API (emits OpenAPI document)..."
dotnet build "$root/src/Hirepath.Api/Hirepath.Api.csproj"

if [ ! -d "$web/node_modules" ]; then
  echo "==> Installing web dependencies..."
  (cd "$web" && npm install)
fi

echo "==> Generating API client..."
(cd "$web" && npm run generate:api)

echo "==> Starting API..."
dotnet run --project "$root/src/Hirepath.Api" &
api_pid=$!

cleanup() {
  echo "==> Stopping API..."
  kill "$api_pid" 2>/dev/null || true
}
trap cleanup EXIT INT TERM

echo "==> Starting web dev server on http://localhost:4200 ..."
(cd "$web" && npm start)
