# Downloads every base image the workshop needs, roughly 1.5 GB in total.
# Run this the evening before on a connection you trust, not in the room.
#
#   .\scripts\prepull.ps1

$ErrorActionPreference = "Continue"

$images = @(
    "mcr.microsoft.com/dotnet/sdk:10.0",
    "mcr.microsoft.com/dotnet/aspnet:10.0",
    "mcr.microsoft.com/dotnet/aspnet:10.0-alpine",
    "node:24-alpine",
    "nginx:1.31-alpine"
)

foreach ($image in $images) {
    Write-Host "Pulling $image"
    docker pull -q $image
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Could not pull $image."
    }
}

Write-Host ""
Write-Host "Done. Building the images now as well means the workshop starts fast:"
Write-Host "  docker compose build"
