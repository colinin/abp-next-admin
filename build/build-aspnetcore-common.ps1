# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$serviceArray = @()

$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.BackendAdmin.HttpApi.Host/"; Service = "backend-admin-api" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.IdentityServer/"; Service = "identity" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.IdentityServer.HttpApi.Host/"; Service = "identity-api" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.LocalizationManagement.HttpApi.Host/"; Service = "localization-api" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.PlatformManagement.HttpApi.Host/"; Service = "platform-api" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.RealtimeMessage.HttpApi.Host/"; Service = "realtime-message-api" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.ApiGateway/"; Service = "internal-apigateway" }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 