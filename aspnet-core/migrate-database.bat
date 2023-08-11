@echo off
cls
set stime=8

start .\migrate-db-cmd.bat LY.MicroService.BackendAdmin.DbMigrator admin --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.AuthServer.DbMigrator auth-server --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.IdentityServer.DbMigrator identityserver4-admin --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.LocalizationManagement.DbMigrator localization --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.Platform.DbMigrator platform --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.RealtimeMessage.DbMigrator messages --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.TaskManagement.DbMigrator task-management --run
ping -n %stime% 127.1 >nul
start .\migrate-db-cmd.bat LY.MicroService.WebhooksManagement.DbMigrator webhooks-management --run
