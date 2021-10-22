# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

$buildSolutionPath = Join-Path $rootFolder "../aspnet-core/"

Set-Location $buildSolutionPath

dotnet build LINGYUN.MicroService.All.sln

# List of solutions used only in development mode
$dependenciesFile = Join-Path $rootFolder "../build/modules.dependencies.json"

function ReadFile($path) {
    return (Get-Content -Raw -Encoding "UTF8" -Path "$path" )
}

function ReadJsonFile($path) {
    $content = ReadFile $path
    return ConvertFrom-Json -InputObject $content
}

$modules = (ReadJsonFile -path $dependenciesFile)

foreach ($module in $modules) {  
    foreach ($dependencieRoot in $module.dependencies) {  
        foreach ($dependencie in $dependencieRoot.dependencies) {  
            $thisPath = Join-Path $rootFolder $dependencieRoot.depPath
            $modulePath = Join-Path $rootFolder $dependencieRoot.path
            Write-host $thisPath
            if (!(Test-Path $modulePath)) {
                New-Item -ItemType Directory -Force -Path $modulePath
            }
            Copy-Item (Join-Path $thisPath $dependencie) -Destination $modulePath
        }
    }
}
