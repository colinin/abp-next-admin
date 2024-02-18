. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $solutionArray) {  
    $packPath = $rootFolder + "/../aspnet-core/Publish/nupkgs"
    dotnet pack -c Release -o $packPath --include-source --include-symbols $solution.File --no-cache
}

Set-Location $rootFolder