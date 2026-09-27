@echo off
title MIAN XITERS — DEPLOY APPLICATION COMMANDS
color 0C
cls
cd /d "%~dp0"
echo ===============================================================================
echo            MIAN XITERS — SLASH COMMANDS REGISTRATION
echo                        dev by 1nOnlyMian
echo ===============================================================================
echo.
echo [*] Deploying /security slash commands to Discord API...
call npm.cmd run deploy-commands
echo.
echo [*] Done!
pause
