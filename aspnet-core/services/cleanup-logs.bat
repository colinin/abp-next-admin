@echo off
cls
chcp 65001

echo. 清理所有服务日志

del .\platform\LINGYUN.Platform.HttpApi.Host\Logs /Q
del .\account\AuthServer.Host\Logs /Q
del .\identity-server\LINGYUN.Abp.IdentityServer4.HttpApi.Host\Logs /Q
del .\messages\LINGYUN.Abp.MessageService.HttpApi.Host\Logs /Q
del .\admin\LINGYUN.Abp.BackendAdmin.HttpApi.Host\Logs /Q
del .\localization\LINGYUN.Abp.LocalizationManagement.HttpApi.Host\Logs /Q

