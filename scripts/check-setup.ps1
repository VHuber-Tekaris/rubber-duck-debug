# Checks that this machine can actually do the workshop, before the workshop.
#
#   .\scripts\check-setup.ps1

$ErrorActionPreference = "Continue"
$problems = @()

Write-Host "Checking the docker CLI..."
$version = docker version --format '{{.Client.Version}}' 2>$null
if ($LASTEXITCODE -ne 0) {
    $problems += "The docker CLI is not on the PATH. Install Docker Desktop, Rancher Desktop or Podman Desktop."
} else {
    Write-Host "  client $version"
}

Write-Host "Checking that the daemon is running..."
$server = docker version --format '{{.Server.Version}}' 2>$null
if ($LASTEXITCODE -ne 0) {
    $problems += "The daemon is not reachable. Start Docker Desktop / Rancher Desktop and wait until it reports running. On Rancher Desktop the container engine has to be set to 'dockerd (moby)'."
} else {
    Write-Host "  server $server"
}

Write-Host "Checking Compose v2..."
$compose = docker compose version --short 2>$null
if ($LASTEXITCODE -ne 0) {
    $problems += "'docker compose' does not work. The old 'docker-compose' is not enough - Compose v2 is required."
} else {
    Write-Host "  compose $compose"
}

Write-Host "Checking that ports 8090 and 8091 are free..."
foreach ($port in 8090, 8091) {
    $used = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue
    if ($used) {
        $problems += "Port $port is already in use by PID $($used[0].OwningProcess). Free it or change the port in compose.yaml."
    } else {
        Write-Host "  $port free"
    }
}

Write-Host ""
if ($problems.Count -eq 0) {
    Write-Host "All good. Start with: docker compose up --build" -ForegroundColor Green
} else {
    Write-Host "Problems found:" -ForegroundColor Yellow
    $problems | ForEach-Object { Write-Host "  - $_" -ForegroundColor Yellow }
}
