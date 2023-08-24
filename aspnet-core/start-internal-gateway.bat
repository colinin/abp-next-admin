@echo off
cls
chcp 65001

title internal-apigateway

echo. 启动内部网关

cd ..\gateways\internal\LINGYUN.MicroService.Internal.ApiGateway\src\LINGYUN.MicroService.Internal.ApiGateway\

if '%1' equ '--publish' goto publish
if '%1' equ '--run' goto run
if '%1' equ '--watchrun' goto watchrun
if '%1' equ '--restore' goto restore
if '%1' equ '' goto run
exit

:publish
dotnet publish -c Release -o ..\..\..\..\..\aspnet-core\services\Publish\internal-apigateway --no-cache --no-restore
copy Dockerfile ..\..\..\..\..\aspnet-core\services\Publish\internal-apigateway\Dockerfile
exit

:run
dotnet run 
exit

:watchrun
dotnet watch run --no-restore
exit

:restore
dotnet restore
exit