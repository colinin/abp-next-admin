. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $solutionArray) {  
    #dotnet pack -c Release --include-source --include-symbols $solution.File --no-cache
    dotnet build -c Release $solution.File --no-cache
}

Set-Location $rootFolder