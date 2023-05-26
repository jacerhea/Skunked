cd /d %~dp0
..\.nuget\NuGet.exe Pack Skunked.nuspec
..\.nuget.exe push Skunked.1.0.0.nupkg