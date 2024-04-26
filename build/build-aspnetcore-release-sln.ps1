. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($solution in $serviceArray) {  
    $publishPath = $rootFolder + "/../aspnet-core/services/Publish/" + $solution.Service
    dotnet publish -c Release -o $publishPath $solution.Path --no-cache
    $dockerFile = Join-Path $solution.Path "Dockerfile";
    if ((Test-Path $dockerFile)) {
        Copy-Item $dockerFile -Destination $publishPath
    }
}

Set-Location $rootFolder