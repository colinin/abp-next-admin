@echo off
cls

call .\migrate-db-cmd.bat LY.MicroService.Platform.DbMigrator platform --run
call .\migrate-db-cmd.bat LY.MicroService.AuthServer.DbMigrator auth-server --run
call .\migrate-db-cmd.bat LY.MicroService.IdentityServer.DbMigrator identityserver4-admin --run
call .\migrate-db-cmd.bat LY.MicroService.LocalizationManagement.DbMigrator localization --run
call .\migrate-db-cmd.bat LY.MicroService.RealtimeMessage.DbMigrator messages --run
call .\migrate-db-cmd.bat LY.MicroService.TaskManagement.DbMigrator task-management --run
call .\migrate-db-cmd.bat LY.MicroService.WebhooksManagement.DbMigrator webhooks-management --run
call .\migrate-db-cmd.bat LY.MicroService.BackendAdmin.DbMigrator admin --run

taskkill /IM dotnet.exe /F