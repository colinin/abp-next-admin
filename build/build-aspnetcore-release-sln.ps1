. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $solutionArray) {  
    $publishPath = $rootFolder + "/../aspnet-core/services/Publish/"
    dotnet publish -c Release -o $publishPath $solution.File --no-cache
}

Set-Location $rootFolder