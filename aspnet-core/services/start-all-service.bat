@echo off
cls

start .\start-identity-server.bat --run
start .\start-apigateway-admin.bat --run
start .\start-platform.bat --run