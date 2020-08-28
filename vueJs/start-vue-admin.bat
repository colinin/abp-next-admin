@echo off
cls
chcp 65001

echo. 启动后台管理UI

if '%1' equ '--run' goto run
if '%1' equ '--build' goto build
if '%1' equ '--publish' goto publish
if '%1' equ '' goto run
exit

:run
npm run serve-nomock
exit


:build
set distPath=.\dist
if exist %distPath% ( rd /s /q %distPath%)
npm run build:prod
exit


:publish
set distPath=.\dist
if exist %distPath% ( rd /s /q %distPath%)
npm run build:prod
copy Dockerfile ..\aspnet-core\services\Publish\client\Dockerfile
set publishPath=..\aspnet-core\services\Publish\client\dist
set nginxPath=..\aspnet-core\services\Publish\client\docker
if exist %publishPath% ( rd /s /q %publishPath%)
if exist %nginxPath% ( rd /s /q %nginxPath%)
xcopy dist\* ..\aspnet-core\services\Publish\client\dist /y /e /i /q
xcopy docker\* ..\aspnet-core\services\Publish\client\docker /y /e /i /q
exit