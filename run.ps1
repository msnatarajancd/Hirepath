#Requires -Version 5.1
<#
    Starts Hirepath: the API (https://localhost:7180) and the Angular dev
    server (http://localhost:4200, proxied to the API). Ctrl+C stops both.
#>
[CmdletBinding()]
param(
    [switch]$SkipInstall
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$web = Join-Path $root 'src/Hirepath.Web'

Write-Host '==> Building API (emits OpenAPI document)...' -ForegroundColor Cyan
dotnet build (Join-Path $root 'src/Hirepath.Api/Hirepath.Api.csproj') | Out-Host

if (-not $SkipInstall -and -not (Test-Path (Join-Path $web 'node_modules'))) {
    Write-Host '==> Installing web dependencies...' -ForegroundColor Cyan
    Push-Location $web; npm install | Out-Host; Pop-Location
}

Write-Host '==> Generating API client...' -ForegroundColor Cyan
Push-Location $web; npm run generate:api | Out-Host; Pop-Location

Write-Host '==> Starting API...' -ForegroundColor Cyan
$api = Start-Process dotnet -ArgumentList 'run', '--project',
    (Join-Path $root 'src/Hirepath.Api') -PassThru -NoNewWindow

try {
    Write-Host '==> Starting web dev server on http://localhost:4200 ...' -ForegroundColor Green
    Push-Location $web
    npm start
}
finally {
    Pop-Location
    if ($api -and -not $api.HasExited) {
        Write-Host '==> Stopping API...' -ForegroundColor Cyan
        Stop-Process -Id $api.Id -Force -ErrorAction SilentlyContinue
    }
}
