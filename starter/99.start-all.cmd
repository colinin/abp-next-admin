@echo off
cls
title start-all
set stime=8
for /f "delims=" %%i in ('dir *.bat *.cmd /b /s^|findstr /v /i "99.start-all.cmd"') do (
    echo %%i
    start %%i
    ping -n %stime% 127.1 >nul
)