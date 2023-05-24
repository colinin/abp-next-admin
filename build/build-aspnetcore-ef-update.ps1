. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($migration in $migrationArray) {    
    Set-Location $migration.Path
    dotnet run
}

Set-Location $rootFolder