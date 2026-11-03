Set WshShell = CreateObject("WScript.Shell")
WshShell.CurrentDirectory = "C:\Users\nafee\Desktop\MIAN XITERS"
WshShell.Run "cmd /c node --max-old-space-size=40 --optimize-for-size --expose-gc dist/index.js", 0, False
