@echo off
cls
chcp 65001

echo. 清理所有服务日志

del .\services\LY.MicroService.Applications.Single\Logs /Q
del .\services\LY.MicroService.BackendAdmin.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.AuthServer\Logs /Q
del .\services\LY.MicroService.AuthServer.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.identityServer\Logs /Q
del .\services\LY.MicroService.identityServer.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.LocalizationManagement.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.PlatformManagement.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.RealtimeMessage.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.TaskManagement.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.WebhooksManagement.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.WechatManagement.HttpApi.Host\Logs /Q
del .\services\LY.MicroService.WorkflowManagement.HttpApi.Host\Logs /Q

