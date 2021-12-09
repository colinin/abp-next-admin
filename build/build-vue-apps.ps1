$rootFolder = (Get-Item -Path "./" -Verbose).FullName

$vuePath = Join-Path $rootFolder "../apps/vue"

$publishPath = Join-Path $rootFolder "../../aspnet-core/services/Publish/client"
$distPath = Join-Path $rootFolder "../../aspnet-core/services/Publish/client/dist"
$dockerPath = Join-Path $rootFolder "../../aspnet-core/services/Publish/client/docker"
Set-Location $vuePath

Remove-Item (Join-Path $vuePath "dist")  -Recurse
Remove-Item $publishPath  -Recurse

CMD /c yarn
CMD /c yarn build

Copy-Item -Path (Join-Path $vuePath "dist") -Destination $distPath -Recurse
Copy-Item -Path (Join-Path $vuePath "docker") -Destination $dockerPath -Recurse
Copy-Item (Join-Path $vuePath "Dockerfile") -Destination $publishPath

Set-Location $rootFolder