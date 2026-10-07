# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$serviceArray = @()
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.LocalizationService/"; Service = "localization-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.AuthServer/"; Service = "auth-server" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.AdminService/"; Service = "admin-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.IdentityService/"; Service = "identity-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.PlatformService/"; Service = "platform-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.TaskService/"; Service = "task-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.MessageService/"; Service = "message-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.WebhookService/"; Service = "webhook-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.WeChatService/"; Service = "wechat-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.WorkflowService/"; Service = "workflow-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.AIService/"; Service = "ai-service" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway/"; Service = "internal-apigateway" }
$serviceArray += [PsObject]@{ Path = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.OpenApi.Gateway/"; Service = "openapi-apigateway" }

[PsObject[]]$dockerArray = @()
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.LocalizationService/Dockerfile"; Image = "labp-localization-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.AuthServer/Dockerfile"; Image = "labp-auth-server"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.AdminService/Dockerfile"; Image = "labp-admin-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.IdentityService/Dockerfile"; Image = "labp-identity-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.PlatformService/Dockerfile"; Image = "labp-platform-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.TaskService/Dockerfile"; Image = "labp-task-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.MessageService/Dockerfile"; Image = "labp-message-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.WebhookService/Dockerfile"; Image = "labp-webhook-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.WeChatService/Dockerfile"; Image = "labp-wechat-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.WorkflowService/Dockerfile"; Image = "labp-workflow-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../aspnet-core/services/LINGYUN.Abp.MicroService.AIService/Dockerfile"; Image = "labp-ai-service"; Version = "10.6.0" }
$dockerArray += [PsObject]@{ Dockerfile = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway/Dockerfile"; Image = "labp-internal-apigateway"; Version = "10.6.0" }

[PsObject[]]$solutionArray = @()
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.All.slnx" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.Aspire.slnx" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../aspnet-core/LINGYUN.MicroService.SingleProject.slnx" }
$solutionArray += [PsObject]@{ File = $rootFolder + "/../gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/LINGYUN.MicroService.Internal.ApiGateway.slnx" }

[PsObject[]]$migrationArray = @()
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.AdminService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.PlatformService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.LocalizationService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.TaskService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.MessageService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.WebhookService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.AIService.DbMigrator" }
$migrationArray += [PsObject]@{ Path = $rootFolder + "/../aspnet-core/migrations/LINGYUN.Abp.MicroService.AuthServer.DbMigrator" }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 
