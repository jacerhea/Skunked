$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
$tempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$workDir = [IO.Path]::GetFullPath((Join-Path $tempRoot ('skunked-package-check-' + [Guid]::NewGuid().ToString('N'))))
if (-not $workDir.StartsWith($tempRoot, [StringComparison]::OrdinalIgnoreCase) -or $workDir -eq $tempRoot) { throw 'Invalid temporary directory.' }
$packages = Join-Path $workDir 'packages'
$consumer = Join-Path $workDir 'consumer'
$version = '0.0.0-localcheck' + [Guid]::NewGuid().ToString('N')
function Invoke-Dotnet([string[]]$arguments) {
    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) { throw "dotnet failed with exit code $LASTEXITCODE" }
}
function Assert-ArchiveEntries([string]$path, [string[]]$expected) {
    $archive = [System.IO.Compression.ZipFile]::OpenRead($path)
    try {
        $names = @($archive.Entries | ForEach-Object { $_.FullName })
        foreach ($name in $expected) { if ($names -notcontains $name) { throw "Missing $name in $path" } }
    } finally { $archive.Dispose() }
}
function Assert-Dependency([string]$path, [string]$id, [string]$versionPart) {
    $archive = [System.IO.Compression.ZipFile]::OpenRead($path)
    try {
        $entry = $archive.Entries | Where-Object { $_.FullName -like '*.nuspec' } | Select-Object -First 1
        if (-not $entry) { throw "Missing nuspec in $path" }
        $reader = [IO.StreamReader]::new($entry.Open())
        try { [xml]$metadata = $reader.ReadToEnd() } finally { $reader.Dispose() }
        $dependencies = $metadata.SelectNodes("//*[local-name()='dependency']")
        $matches = @($dependencies | Where-Object { $_.id -eq $id -and $_.version -like "*$versionPart*" })
        if ($matches.Count -eq 0) { throw "Missing expected dependency $id $versionPart in $path" }
    } finally { $archive.Dispose() }
}

try {
    New-Item -ItemType Directory -Path $packages, $consumer -Force | Out-Null
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    Invoke-Dotnet @('pack', (Join-Path $repo 'src/Skunked/Skunked.csproj'), '-c', 'Release', '-o', $packages, "-p:Version=$version", '-p:NuGetAudit=false')
    Invoke-Dotnet @('pack', (Join-Path $repo 'src/Skunked.AI/Skunked.AI.fsproj'), '-c', 'Release', '-o', $packages, "-p:Version=$version", '-p:NuGetAudit=false')
    $core = Join-Path $packages "Skunked.$version.nupkg"
    $ai = Join-Path $packages "Skunked.AI.$version.nupkg"
    $symbols = Join-Path $packages "Skunked.$version.snupkg"
    Assert-ArchiveEntries $core @('lib/net8.0/Skunked.dll','lib/net8.0/Skunked.xml','lib/net9.0/Skunked.dll','lib/net9.0/Skunked.xml','lib/net10.0/Skunked.dll','lib/net10.0/Skunked.xml','Skunked.nuspec','README.md')
    Assert-ArchiveEntries $ai @('lib/net10.0/Skunked.AI.dll','lib/net10.0/Skunked.AI.xml','Skunked.AI.nuspec','README.md')
    Assert-ArchiveEntries $symbols @('lib/net8.0/Skunked.pdb','lib/net9.0/Skunked.pdb','lib/net10.0/Skunked.pdb')
    Assert-Dependency $core 'Combinatorics' '2.0.0'
    Assert-Dependency $ai 'Skunked' $version
    $project = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework></PropertyGroup>
  <ItemGroup><PackageReference Include="Skunked" Version="$version" /><PackageReference Include="Skunked.AI" Version="$version" /></ItemGroup>
</Project>
"@
    Set-Content -LiteralPath (Join-Path $consumer 'Consumer.csproj') -Value $project -Encoding UTF8
    Set-Content -LiteralPath (Join-Path $consumer 'Program.cs') -Value 'using Skunked; var game = new Cribbage(new[] { 1, 2 }, new GameRules()); System.Console.WriteLine(game.State.PlayerIds.Count); System.Reflection.Assembly.Load("Skunked.AI");' -Encoding UTF8
    $escapedPackages = [Security.SecurityElement]::Escape($packages)
    $config = '<configuration><packageSources><clear /><add key="local" value="' + $escapedPackages + '" /><add key="nuget" value="https://api.nuget.org/v3/index.json" /></packageSources></configuration>'
    $configPath = Join-Path $workDir 'NuGet.Config'
    Set-Content -LiteralPath $configPath -Value $config -Encoding UTF8
    Invoke-Dotnet @('restore', (Join-Path $consumer 'Consumer.csproj'), '--configfile', $configPath, '-p:NuGetAudit=false')
    Invoke-Dotnet @('run', '--project', (Join-Path $consumer 'Consumer.csproj'), '--no-restore')
    Write-Output 'Package archives and consumer smoke test passed.'
} finally {
    if (Test-Path -LiteralPath $workDir) { Remove-Item -LiteralPath $workDir -Recurse -Force }
}
