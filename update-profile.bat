@echo off
title MIAN XITERS — AUTO UPDATE BOT NAME AND AVATAR
color 0C
cls
cd /d "%~dp0"
echo ===============================================================================
echo            MIAN XITERS — BOT IDENTITY SYNC & AVATAR UPLOAD
echo                        dev by 1nOnlyMian
echo               MIAN XITERS ULTIMATE DOMINANCE !
echo ===============================================================================
echo.
echo [*] Applying name "MIAN XITERS" and uploading custom avatar logo...
call npm.cmd run update-profile
echo.
pause
