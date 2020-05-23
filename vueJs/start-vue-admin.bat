@echo off
cls
chcp 65001

echo. 启动后台管理UI

if '%1' equ '--run' goto run
if '%1' equ '' goto run
exit

:run
npm run serve-nomock
pause
exit
