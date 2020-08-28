@echo off
cls
chcp 65001

echo. 启动身份认证服务

cd .\account\AuthServer.Host

if '%1' equ '--publish' goto publish
if '%1' equ '--run' goto run
if '%1' equ '--restore' goto restore
if '%1' equ '' goto run
exit

:publish
dotnet publish -c Release -o ..\..\Publish\identityserver --no-cache --no-restore
copy Dockerfile ..\..\Publish\identityserver\Dockerfile
exit

:run
dotnet run 
exit

:restore
dotnet restore
exit