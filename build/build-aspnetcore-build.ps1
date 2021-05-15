. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($service in $serviceArray) {  
    $copyFromConfig = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($rootFolder + $service.ConfigPath + $service.ConfigFile)
    if (Test-Path $copyFromConfig) 
    {
        $copyToConfig = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($service.Path + $service.ConfigFile)
        $configExists = Test-Path $copyToConfig
        if ($configExists -eq $false) {
            Write-host ""
            Write-host "Coping appsettings.Development.json ..." -ForegroundColor red -BackgroundColor  yellow
            Write-host "" 
            Copy-Item $copyFromConfig $copyToConfig
        }
    }
      
    Set-Location $service.Path
    dotnet build --no-cache
}

Set-Location $rootFolder