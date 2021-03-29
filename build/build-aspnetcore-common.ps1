# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$serviceArray = @()

$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/account/AuthServer.Host"; Service = "identityserver" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/admin/LINGYUN.Abp.BackendAdmin.HttpApi.Host"; Service = "admin" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/identity-server/LINGYUN.Abp.IdentityServer4.HttpApi.Host"; Service = "identityserver4-admin" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/apigateway/LINGYUN.ApiGateway.Host"; Service = "apigateway-host" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/apigateway/LINGYUN.ApiGateway.HttpApi.Host"; Service = "apigateway-admin" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/messages/LINGYUN.Abp.MessageService.HttpApi.Host"; Service = "messages" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/platform/LINGYUN.Platform.HttpApi.Host"; Service = "platform" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/localization/LINGYUN.Abp.LocalizationManagement.HttpApi.Host"; Service = "localization" }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 