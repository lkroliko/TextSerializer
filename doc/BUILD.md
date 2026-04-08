# Build nuget

## Automated (recommended)

Run from the repository root:

```powershell
.\build.ps1 -Version 1.2.0
```

The script will:
1. Update version in `TextSerializer.csproj` (FileVersion, Version, AssemblyVersion, PackageVersion)
2. Update version in `TextSerializer.nuspec`
3. Update `CHANGELOG.md` with the new version and date
4. Run all tests
5. Build in Release configuration
6. Pack NuGet package to `artifacts/` directory

## Manual

1. Edit `src\TextSerializer\TextSerializer.nuspec`, change `<version>`
2. Edit `src\TextSerializer\TextSerializer.csproj`, change `FileVersion`, `Version`, `AssemblyVersion`, `PackageVersion`
3. Run tests
4. Build in Release and pack NuGet