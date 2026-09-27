@echo off
title MIAN XITERS — ULTIMATE ENTERPRISE SECURITY BOT
color 0C
cls
echo ===============================================================================
echo            MIAN XITERS — ADVANCE HIGH-END SECURITY BOT
echo                        dev by 1nOnlyMian
echo               MIAN XITERS ULTIMATE DOMINANCE !
echo ===============================================================================
echo.

cd /d "%~dp0"

if not exist .env (
    echo [!] .env file not found! Copying from .env.example...
    copy .env.example .env
    echo [!] Please open .env and add your DISCORD_TOKEN before starting.
    notepad .env
    pause
    exit /b
)

echo [*] Checking node environment...
node -v >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Node.js is not installed or not in PATH! Please install Node.js v20+.
    pause
    exit /b
)

if not exist dist\index.js (
    echo [*] Compiling TypeScript code...
    call npm.cmd run build
)

echo [*] Launching MIAN XITERS Autonomous Gateway Defense Engines...
echo.
node dist/index.js
pause
