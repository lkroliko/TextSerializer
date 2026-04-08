param(
    [Parameter(Mandatory = $true)]
    [string]$Version
)

$ErrorActionPreference = "Stop"

if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    Write-Error "Version must be in format X.Y.Z (e.g. 1.2.0)"
    exit 1
}

$solutionDir = $PSScriptRoot
$csprojPath = Join-Path $solutionDir "src\TextSerializer\TextSerializer.csproj"
$nuspecPath = Join-Path $solutionDir "src\TextSerializer\TextSerializer.nuspec"
$changelogPath = Join-Path $solutionDir "doc\CHANGELOG.md"

# --- Update .csproj version ---
Write-Host "Updating version in TextSerializer.csproj to $Version..." -ForegroundColor Cyan
$csproj = Get-Content $csprojPath -Raw
$csproj = $csproj -replace '<FileVersion>[^<]+</FileVersion>',    "<FileVersion>$Version</FileVersion>"
$csproj = $csproj -replace '<Version>[^<]+</Version>',            "<Version>$Version</Version>"
$csproj = $csproj -replace '<AssemblyVersion>[^<]+</AssemblyVersion>', "<AssemblyVersion>$Version</AssemblyVersion>"
$csproj = $csproj -replace '<PackageVersion>[^<]+</PackageVersion>', "<PackageVersion>$Version</PackageVersion>"
Set-Content $csprojPath $csproj -NoNewline

# --- Update .nuspec version ---
Write-Host "Updating version in TextSerializer.nuspec to $Version..." -ForegroundColor Cyan
$nuspec = Get-Content $nuspecPath -Raw
$nuspec = $nuspec -replace '<version>[^<]+</version>', "<version>$Version</version>"
Set-Content $nuspecPath $nuspec -NoNewline

# --- Update CHANGELOG.md ---
Write-Host "Updating CHANGELOG.md..." -ForegroundColor Cyan
$changelog = Get-Content $changelogPath -Raw
$date = Get-Date -Format "yyyy-MM-dd"
$changelog = $changelog -replace '#### Next version', "#### Next version`n`n### $Version ($date)"
Set-Content $changelogPath $changelog -NoNewline

# --- Run tests ---
Write-Host "Running tests..." -ForegroundColor Cyan
dotnet test (Join-Path $solutionDir "TextSerializer.sln") --configuration Release --verbosity minimal
if ($LASTEXITCODE -ne 0) {
    Write-Error "Tests failed. Aborting build."
    exit 1
}

# --- Build Release ---
Write-Host "Building Release..." -ForegroundColor Cyan
dotnet build (Join-Path $solutionDir "src\TextSerializer\TextSerializer.csproj") --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed."
    exit 1
}

# --- Pack NuGet ---
Write-Host "Packing NuGet..." -ForegroundColor Cyan
$outputDir = Join-Path $solutionDir "artifacts"
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}

nuget pack $nuspecPath -OutputDirectory $outputDir -BasePath (Join-Path $solutionDir "src\TextSerializer")
if ($LASTEXITCODE -ne 0) {
    # Fallback to dotnet pack if nuget CLI is not available
    Write-Host "nuget CLI not found, trying dotnet pack..." -ForegroundColor Yellow
    dotnet pack $csprojPath --configuration Release --no-build --output $outputDir
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Pack failed."
        exit 1
    }
}

Write-Host ""
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "Version: $Version" -ForegroundColor Green
Write-Host "Package output: $outputDir" -ForegroundColor Green
