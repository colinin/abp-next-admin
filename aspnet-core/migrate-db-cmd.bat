@echo off
chcp 65001

title %2

@echo %2 migrating

cd .\migrations\%1

if '%3' equ '--run' goto run
if '%3' equ '--restore' goto restore
if '%3' equ '--ef-u' goto efu
if '%3' equ '' goto run
exit

:run
start cmd.exe /c dotnet run --no-build
goto end

:restore
dotnet restore
goto end

:efu
dotnet ef database update
goto end

:end
cd ..\..\
@echo %2 migrated
@echo --------
