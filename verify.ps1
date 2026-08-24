#Requires -Version 5.1
<#
    Runs every verification gate. Fails fast on the first red gate.
    Use -SkipE2E to skip the Playwright flow (e.g. no browsers installed).
#>
[CmdletBinding()]
param(
    [switch]$SkipE2E
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$web = Join-Path $root 'src/Hirepath.Web'

function Step($name, [scriptblock]$body) {
    Write-Host ""
    Write-Host "==> $name" -ForegroundColor Cyan
    & $body
    if ($LASTEXITCODE -ne 0) {
        Write-Host "FAILED: $name" -ForegroundColor Red
        exit 1
    }
}

Step 'Backend build (warnings as errors)' { dotnet build (Join-Path $root 'Hirepath.slnx') }
Step 'Backend format check' { dotnet format (Join-Path $root 'Hirepath.slnx') --verify-no-changes }
Step 'Backend tests' { dotnet test (Join-Path $root 'Hirepath.slnx') --no-build }

Step 'Generate API client' { Push-Location $web; npm run generate:api; $script:c = $LASTEXITCODE; Pop-Location; $global:LASTEXITCODE = $script:c }
Step 'Frontend lint' { Push-Location $web; npm run lint; $script:c = $LASTEXITCODE; Pop-Location; $global:LASTEXITCODE = $script:c }
Step 'Frontend format check' { Push-Location $web; npm run format:check; $script:c = $LASTEXITCODE; Pop-Location; $global:LASTEXITCODE = $script:c }
Step 'Frontend tests' { Push-Location $web; npm run test:ci; $script:c = $LASTEXITCODE; Pop-Location; $global:LASTEXITCODE = $script:c }
Step 'Frontend production build' { Push-Location $web; npm run build; $script:c = $LASTEXITCODE; Pop-Location; $global:LASTEXITCODE = $script:c }

if (-not $SkipE2E) {
    $e2e = Join-Path $root 'tests/Hirepath.E2E'
    Step 'End-to-end (Playwright, Demo Mode)' { Push-Location $e2e; npm test; $script:c = $LASTEXITCODE; Pop-Location; $global:LASTEXITCODE = $script:c }
}


Write-Host ""
Write-Host "All gates passed." -ForegroundColor Green
