# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$serviceArray = @()

$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/account/AuthServer.Host/"; Service = "identityserver"; ConfigPath = "/../aspnet-core/configuration/account/AuthServer.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/admin/LINGYUN.Abp.BackendAdmin.HttpApi.Host/"; Service = "admin"; ConfigPath = "/../aspnet-core/configuration/admin/LINGYUN.Abp.BackendAdmin.HttpApi.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/identity-server/LINGYUN.Abp.IdentityServer4.HttpApi.Host/"; Service = "identityserver4-admin"; ConfigPath = "/../aspnet-core/configuration/identity-server/LINGYUN.Abp.IdentityServer4.HttpApi.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/apigateway/LINGYUN.ApiGateway.Host/"; Service = "apigateway-host"; ConfigPath = "/../aspnet-core/configuration/apigateway/LINGYUN.ApiGateway.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/apigateway/LINGYUN.ApiGateway.HttpApi.Host/"; Service = "apigateway-admin"; ConfigPath = "/../aspnet-core/configuration/apigateway/LINGYUN.ApiGateway.HttpApi.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/messages/LINGYUN.Abp.MessageService.HttpApi.Host/"; Service = "messages"; ConfigPath = "/../aspnet-core/configuration/messages/LINGYUN.Abp.MessageService.HttpApi.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/platform/LINGYUN.Platform.HttpApi.Host/"; Service = "platform"; ConfigPath = "/../aspnet-core/configuration/platform/LINGYUN.Platform.HttpApi.Host/"; ConfigFile = "appsettings.Development.json" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/localization/LINGYUN.Abp.LocalizationManagement.HttpApi.Host/"; Service = "localization"; ConfigPath = "/../aspnet-core/configuration/localization/LINGYUN.Abp.LocalizationManagement.HttpApi.Host/"; ConfigFile = "appsettings.Development.json" }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 