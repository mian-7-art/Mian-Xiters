# WPF Project Build Rules

## Build Procedure
- **MSBuild Toolchain**: Always use Visual Studio `MSBuild.exe` (found via vswhere or `C:\Program Files\Microsoft Visual Studio\...\MSBuild.exe`) rather than `dotnet build` when compiling this `.NET Framework 4.7.2` WPF application. `dotnet build` can skip XAML Markup Compiler passes (`MarkupCompilePass1` / `MarkupCompilePass2`), causing missing UI element errors (`CS0103`) in generated `MainWindow.g.cs`.
- **Cache Clean**: If XAML elements or code-behind bindings are missing or updated, clean `obj/` and `bin/` directories prior to building to ensure full XAML re-compilation.
- **Output Artifacts**: Keep build outputs clean; ensure build targets output cleanly to `bin/Debug` without lingering old executable releases.
