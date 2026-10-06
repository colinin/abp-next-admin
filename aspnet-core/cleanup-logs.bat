@echo off
cls
chcp 65001

echo. 清理所有服务日志

del .\aspire\LINGYUN.Abp.MicroService.AdminService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.AIService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.ApiGateway\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.AuthServer\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.IdentityService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.LocalizationService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.MessageService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.PlatformService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.TaskService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.WebhookService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.WeChatService\Logs /Q
del .\aspire\LINGYUN.Abp.MicroService.WorkflowService\Logs /Q

del .\services\LY.MicroService.Applications.Single\Logs /Q
del .\services\LINGYUN.Abp.MicroService.LocalizationService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.AuthServer\Logs /Q
del .\services\LINGYUN.Abp.MicroService.AdminService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.IdentityService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.PlatformService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.TaskService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.MessageService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.WebhookService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.WeChatService\Logs /Q
del .\services\LINGYUN.Abp.MicroService.WorkflowService\Logs /Q


