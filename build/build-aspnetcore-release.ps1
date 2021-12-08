. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($service in $serviceArray) {    
    Set-Location $service.Path
    $publishPath = $rootFolder + "/aspnet-core/services/Publish/" + $service.Service
    dotnet publish -c Release -o $publishPath --no-cache --no-restore
    Copy-Item (Join-Path $service.Path "Dockerfile") -Destination $publishPath -Recurse
}

Set-Location $rootFolder