@echo off
cls

start .\start-http-api-host.bat LY.MicroService.IdentityServer identityserver --run
start .\start-http-api-host.bat LY.MicroService.IdentityServer.HttpApi.Host identityserver4-admin --run
start .\start-http-api-host.bat LY.MicroService.LocalizationManagement.HttpApi.Host localization --run
start .\start-http-api-host.bat LY.MicroService.PlatformManagement.HttpApi.Host platform --run
start .\start-http-api-host.bat LY.MicroService.RealtimeMessage.HttpApi.Host messages --run
start .\start-http-api-host.bat LY.MicroService.TaskManagement.HttpApi.Host task-management --run
start .\start-http-api-host.bat LY.MicroService.BackendAdmin.HttpApi.Host admin --run
ping -n 10 127.1 >nul
start .\start-internal-gateway.bat --run