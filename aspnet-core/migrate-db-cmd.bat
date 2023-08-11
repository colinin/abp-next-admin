@echo off
cls
chcp 65001

title %2

@echo %2 migration running

cd .\migrations\%1

if '%3' equ '--run' goto run
if '%3' equ '--restore' goto restore
if '%3' equ '--ef-u' goto efu
if '%3' equ '' goto run
exit

:run
dotnet run 
pause
exit

:restore
dotnet restore
exit

:efu
dotnet ef databse update
exit