cd /d %~dp0
..\.nuget\NuGet.exe Pack Skunked.nuspec
..\.nuget.exe push Skunked.0.9.6.nupkg