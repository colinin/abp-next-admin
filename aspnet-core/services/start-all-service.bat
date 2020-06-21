@echo off
cls

start .\start-identity-server.bat --run
start .\start-apigateway-admin.bat --run
start .\start-platform.bat --run
start .\start-messages.bat --run
ping -n 10 127.1 >nul
start .\start-apigateway-host.bat --run