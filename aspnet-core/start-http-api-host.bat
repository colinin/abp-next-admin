@echo off
cls
chcp 65001

title %2-host
@echo %2-host

cd .\services\%1

if '%3' equ '--publish' goto publish
if '%3' equ '--watchrun' goto watchrun
if '%3' equ '--run' goto run
if '%3' equ '--restore' goto restore
if '%3' equ '--ef-u' goto efu
if '%3' equ '' goto run
exit

:publish
dotnet publish -c Release -o .\services\Publish\%2 --no-cache --no-restore
copy Dockerfile .\services\Publish\%2\Dockerfile
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

:efu
dotnet ef databse update
exit