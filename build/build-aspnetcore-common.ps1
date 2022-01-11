# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$serviceArray = @()

$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.BackendAdmin.HttpApi.Host/"; Service = "admin" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.IdentityServer/"; Service = "identityserver" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.IdentityServer.HttpApi.Host/"; Service = "identityserver4-admin" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.LocalizationManagement.HttpApi.Host/"; Service = "localization" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.PlatformManagement.HttpApi.Host/"; Service = "platform" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.RealtimeMessage.HttpApi.Host/"; Service = "messages" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.TaskManagement.HttpApi.Host/"; Service = "task-management" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.ApiGateway/"; Service = "internal-apigateway" }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 