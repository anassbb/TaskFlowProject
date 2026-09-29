<#
.SYNOPSIS
    Compile la solution et lance tous les tests en passant par dotnet.exe.
.DESCRIPTION
    Sur le poste de travail, la politique de sécurité bloque l'exécution des .exe générés dans le profil
    utilisateur, donc "dotnet test" échoue avec "Accès refusé". Ce script lance chaque projet de test
    via "dotnet <projet>.dll", ce qui est autorisé. En CI (GitHub Actions), "dotnet test" suffit.
.EXAMPLE
    ./scripts/test.ps1
    ./scripts/test.ps1 -NoBuild
#>
param([switch]$NoBuild)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

if (-not $NoBuild) {
    dotnet build (Join-Path $root 'TaskFlow.slnx') -nologo -v q
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

$failed = 0
foreach ($proj in Get-ChildItem (Join-Path $root 'tests') -Recurse -Filter '*Tests.csproj') {
    $dll = Join-Path $proj.DirectoryName "bin/Debug/net10.0/$($proj.BaseName).dll"
    Write-Host "`n=== $($proj.BaseName) ===" -ForegroundColor Cyan
    dotnet $dll
    if ($LASTEXITCODE -ne 0) { $failed++ }
}

if ($failed -gt 0) { Write-Host "`n$failed projet(s) de test en échec." -ForegroundColor Red; exit 1 }
Write-Host "`nTous les tests passent." -ForegroundColor Green
