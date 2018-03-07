# temporarily change to the correct folder
Push-Location $folder

# do stuff, call ant, etc

# now back to previous directory
Pop-Location

dotnet pack /src/Skunked.Standard/Skunked.Standard.csproj
