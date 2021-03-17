. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($service in $serviceArray) {    
    Set-Location $service.Path
    $publishPath = $service.Path + "/../../Publish/" + $service.Service
    dotnet publish -c Release -o $publishPath --no-cache --no-restore
}

Set-Location $rootFolder