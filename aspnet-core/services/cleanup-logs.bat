@echo off
cls
chcp 65001

echo. 清理所有服务日志

del .\platform\LINGYUN.Platform.HttpApi.Host\Logs /Q
del .\apigateway\LINGYUN.ApiGateway.Host\Logs /Q
del .\apigateway\LINGYUN.ApiGateway.HttpApi.Host\Logs /Q
del .\account\AuthServer.Host\Logs /Q
del .\messages\LINGYUN.Abp.MessageService.HttpApi.Host\Logs /Q
del .\admin\LINGYUN.BackendAdminApp.Host\Logs /Q

