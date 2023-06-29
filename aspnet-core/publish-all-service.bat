echo 将各个服务编译并发布到./services/publish目录下
@echo off
cls

start .\start-http-api-host.bat LY.MicroService.IdentityServer identityserver --publish
start .\start-http-api-host.bat LY.MicroService.IdentityServer.HttpApi.Host identityserver4-admin --publish
start .\start-http-api-host.bat LY.MicroService.LocalizationManagement.HttpApi.Host localization --publish
start .\start-http-api-host.bat LY.MicroService.PlatformManagement.HttpApi.Host platform --publish
start .\start-http-api-host.bat LY.MicroService.RealtimeMessage.HttpApi.Host messages --publish
start .\start-http-api-host.bat LY.MicroService.TaskManagement.HttpApi.Host task-management --publish
start .\start-http-api-host.bat LY.MicroService.BackendAdmin.HttpApi.Host admin --publish
start .\start-http-api-host.bat LY.MicroService.WorkflowManagement.HttpApi.Host workflow --publish
start .\start-http-api-host.bat LY.MicroService.WebhooksManagement.HttpApi.Host webhooks --publish
ping -n 10 127.1 >nul
start .\start-internal-gateway.bat --publish