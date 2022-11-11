. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $solutionArray) {  
    dotnet build $solution.File --no-cache
}

Set-Location $rootFolder