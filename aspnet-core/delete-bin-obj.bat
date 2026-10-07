@echo off
setlocal enabledelayedexpansion

cls
echo Deleting all BIN and OBJ folders...

for /f "delims=" %%D in ('dir /s /b /ad bin obj 2^>nul') do (
    echo %%D | findstr /i "\\node_modules\\" >nul
    if errorlevel 1 (
        echo Deleting: %%D
        rd /s /q "%%D"
    ) else (
        echo Skipping: %%D
    )
)

echo BIN and OBJ folders have been successfully deleted.
endlocal