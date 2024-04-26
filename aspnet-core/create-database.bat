@echo off
cls

call .\migrate-db-cmd.bat LY.MicroService.Platform.EntityFrameworkCore platform --ef-u
call .\migrate-db-cmd.bat LY.MicroService.BackendAdmin.EntityFrameworkCore admin --ef-u
call .\migrate-db-cmd.bat LY.MicroService.AuthServer.EntityFrameworkCore auth-server --ef-u
call .\migrate-db-cmd.bat LY.MicroService.IdentityServer.EntityFrameworkCore identityserver4-admin --ef-u
call .\migrate-db-cmd.bat LY.MicroService.LocalizationManagement.EntityFrameworkCore localization --ef-u
call .\migrate-db-cmd.bat LY.MicroService.RealtimeMessage.EntityFrameworkCore messages --ef-u
call .\migrate-db-cmd.bat LY.MicroService.TaskManagement.EntityFrameworkCore task-management --ef-u
call .\migrate-db-cmd.bat LY.MicroService.WebhooksManagement.EntityFrameworkCore webhooks-management --ef-u

taskkill /IM dotnet.exe /F