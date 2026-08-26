# Puts the repository into the state it should be in after a given task, so a
# task you could not finish does not block you from the rest of the workshop.
#
#   .\checkpoints\apply.ps1 2     # tasks 1 and 2 fixed -> start of task 3
#   .\checkpoints\apply.ps1 4     # tasks 1-4 fixed     -> start of task 5
#
# This overwrites your own versions of those files. There is no undo other than
# git, so commit or stash anything you want to keep first.

param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateRange(1, 4)]
    [int] $Task
)

$ErrorActionPreference = "Stop"

$source = Join-Path $PSScriptRoot "after-task-$Task"
$repo = Split-Path $PSScriptRoot -Parent

if (-not (Test-Path $source)) {
    throw "No checkpoint for task $Task at $source."
}

Write-Host "Copying the state after task $Task over your working copy:"
Get-ChildItem -Recurse -File -Force $source | ForEach-Object {
    $relative = $_.FullName.Substring($source.Length + 1)
    Write-Host "  $relative"
}

Copy-Item -Path (Join-Path $source "*") -Destination $repo -Recurse -Force

Write-Host ""
Write-Host "Done. You are now at the start of task $($Task + 1)."
Write-Host "Next: docker compose up -d --build"
