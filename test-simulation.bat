@echo off
title MIAN XITERS — AUTONOMOUS DEFENSE SIMULATION & SCAN
color 0A
cls
cd /d "%~dp0"
echo ===============================================================================
echo            MIAN XITERS — DEFENSE STRESS TEST & ATTACK SCAN
echo                        dev by 1nOnlyMian
echo               MIAN XITERS ULTIMATE DOMINANCE !
echo ===============================================================================
echo.
echo [*] Executing offline attack simulation suite...
echo.
call npm.cmd run test-simulation
echo.
pause
