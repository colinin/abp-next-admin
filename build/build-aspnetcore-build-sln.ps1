. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $solutionArray) {  
    dotnet build $solution.File -c Release --no-cache
}

Set-Location $rootFolder