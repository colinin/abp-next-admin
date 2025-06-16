. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $solutionArray) {  
    dotnet pack $solution.File -c Release --include-source --include-symbols
    #dotnet build -c Release $solution.File --no-cache
}

Set-Location $rootFolder