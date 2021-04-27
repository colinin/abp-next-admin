. "./build-aspnetcore-common.ps1"

# update volo.abp.cli
dotnet tool update -g Volo.Abp.Cli

# update all solutions
$solutionPath = $rootFolder + "/../aspnet-core/"

Set-Location $solutionPath
abp update

Set-Location $rootFolder