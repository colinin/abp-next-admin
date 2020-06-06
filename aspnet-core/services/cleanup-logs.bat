@echo off
cls
chcp 65001

echo. 清理所有服务日志

del .\platform\LINGYUN.Platform.HttpApi.Host\Logs /y
del .\apigateway\LINGYUN.ApiGateway.Host\Logs /y
del .\apigateway\LINGYUN.ApiGateway.HttpApi.Host\Logs /y
del .\account\AuthServer.Host\Logs /y

