# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$serviceArray = @()

$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.BackendAdmin.HttpApi.Host/"; Service = "admin-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.AuthServer/"; Service = "auth-server" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.AuthServer.HttpApi.Host/"; Service = "auth-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.identityServer/"; Service = "identity-server" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.identityServer.HttpApi.Host/"; Service = "identity-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.LocalizationManagement.HttpApi.Host/"; Service = "localization-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.PlatformManagement.HttpApi.Host/"; Service = "platform-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.RealtimeMessage.HttpApi.Host/"; Service = "message-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.TaskManagement.HttpApi.Host/"; Service = "task-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.WebhooksManagement.HttpApi.Host/"; Service = "webhook-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.WorkflowManagement.HttpApi.Host/"; Service = "workflow-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LY.MicroService.WechatManagement.HttpApi.Host/"; Service = "wechat-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway/"; Service = "internal-apigateway" }

[PsObject[]]$solutionArray = @()
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.All.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.Common.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.TaskManagement.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.WebhooksManagement.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.Workflow.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.SingleProject.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.WechatManagement.sln" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/LINGYUN.MicroService.Internal.ApiGateway.sln" }

[PsObject[]]$migrationArray = @()
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.Platform.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.LocalizationManagement.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.RealtimeMessage.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.IdentityServer.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.TaskManagement.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.AuthServer.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.WebhooksManagement.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.BackendAdmin.DbMigrator" }
#$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LY.MicroService.Applications.Single.DbMigrator" }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 
