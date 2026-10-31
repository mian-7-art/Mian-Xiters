Set WshShell = CreateObject("WScript.Shell")
WshShell.CurrentDirectory = "C:\Users\nafee\Desktop\MIAN XITERS"
WshShell.Run "cmd /c node dist/index.js", 0, False
