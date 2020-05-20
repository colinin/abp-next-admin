@echo off
cls

start .\start-identity-server.bat --run
start .\start-apigateway-admin.cmd --run
start .\start-apigateway-host.cmd --run