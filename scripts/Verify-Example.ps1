$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
$readme = [IO.File]::ReadAllText((Join-Path $repo 'README.md')).Replace("`r`n", "`n")
$example = [IO.File]::ReadAllText((Join-Path $repo 'examples/GettingStarted/Program.cs')).Replace("`r`n", "`n").Trim()
$match = [regex]::Match($readme, '(?s)```csharp\n(.*?)\n```')
if (-not $match.Success -or $match.Groups[1].Value.Trim() -ne $example) {
    throw 'README C# example does not match examples/GettingStarted/Program.cs.'
}
Write-Output 'README example matches the executable sample.'
