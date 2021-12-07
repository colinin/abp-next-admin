@echo off
cls

start .\start-auth-server.bat --run
start .\start-identity-server.bat --run
start .\start-backend-admin.bat --run
start .\start-localization.bat --run
start .\start-messages.bat --run
start .\start-platform.bat --run