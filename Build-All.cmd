@echo off
setlocal

powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -File "%~dp0Build-All.ps1"
set "BUILD_EXIT_CODE=%ERRORLEVEL%"

echo.
if not "%BUILD_EXIT_CODE%"=="0" (
    echo Build failed. Review the messages above.
) else (
    echo Build completed successfully.
)

pause
exit /b %BUILD_EXIT_CODE%
