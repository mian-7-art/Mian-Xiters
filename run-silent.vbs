Set WshShell = CreateObject("WScript.Shell")
WshShell.CurrentDirectory = "C:\Users\nafee\Desktop\MIAN XITERS"
WshShell.Run "node dist/index.js", 0, False
