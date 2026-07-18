. "./build-aspnetcore-common.ps1"

[xml]$xml = Get-Content -Path "../common.props"
$publishVersion = $xml.Project.PropertyGroup.Version[0].Trim()
# $publishVersion = $xml.Project.PropertyGroup.Version[0].Trim() + "-rc1"
# Build all solutions
foreach ($service in $serviceArray) {    
    Set-Location $service.Path
    $publishPath = $rootFolder + "/../aspnet-core/services/Publish/" + $service.Service
    dotnet publish -c Release -o $publishPath --no-cache
    Remove-Item (Join-Path $publishPath "appsettings.Development.json")  -Recurse
    Remove-Item (Join-Path $publishPath "appsettings.Production.json")  -Recurse
    Remove-Item (Join-Path $publishPath "appsettings.Staging.json")  -Recurse
    Copy-Item (Join-Path $service.Path "Dockerfile") -Destination $publishPath -Recurse
    Copy-Item (Join-Path $service.Path "openiddict.pfx") -Destination $publishPath -Recurse
    docker build -t "lvjia/$($service.Service):$publishVersion" $publishPath
    docker tag "lvjia/$($service.Service):$publishVersion" 99.22.20.2:8082/"lvjia/$($service.Service):$publishVersion"
    docker push 99.22.20.2:8082/"lvjia/$($service.Service):$publishVersion"
}

Set-Location $rootFolder
