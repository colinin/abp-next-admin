@echo off
cls
chcp 65001

echo. 启动消息服务

cd .\messages\LINGYUN.Abp.MessageService.HttpApi.Host

if '%1' equ '--publish' goto publish
if '%1' equ '--run' goto run
if '%1' equ '--restore' goto restore
if '%1' equ '--ef-u' goto efu
if '%1' equ '' goto run
exit

:publish
dotnet publish -c Release -o ..\..\Publish\messages --no-cache --no-restore
copy Dockerfile ..\..\Publish\messages\Dockerfile
exit

:run
dotnet run 
exit

:restore
dotnet restore
exit

:efu
dotnet ef database update
exit