using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WpfApp1
{
    /// <summary>
    /// Custom entry point that hooks AssemblyResolve BEFORE any WPF or
    /// third-party types are loaded.  This guarantees that the embedded
    /// Newtonsoft.Json / AuthlyX / BouncyCastle DLLs are resolved from
    /// the exe's embedded resources even when the user distributes the
    /// single exe without any companion DLLs.
    /// </summary>
    public static class Program
    {
        private static readonly Dictionary<string, Assembly> _resolvedAssemblies =
            new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        [STAThread]
        public static void Main()
        {
            // Register Global Crash & Exception Catchers
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    DiagnosticLogger.LogCrash(ex, "AppDomain Unhandled Exception");
                }
            };

            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                if (args.Exception != null)
                {
                    DiagnosticLogger.LogCrash(args.Exception, "Unobserved Task Exception");
                    args.SetObserved();
                }
            };

            // Security Check — Exit immediately if debugger, dumper, or reverse engineering tool is attached
            if (!SecurityGuard.ValidateEnvironment())
            {
                Environment.Exit(0);
                return;
            }

            // Hook FIRST — before any other managed type is touched
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            // Start continuous background security watcher
            SecurityGuard.StartContinuousProtection();

            // Now it is safe to launch WPF
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            try
            {
                var assemblyName = new AssemblyName(args.Name).Name;
                if (string.IsNullOrEmpty(assemblyName)) return null;

                lock (_resolvedAssemblies)
                {
                    if (_resolvedAssemblies.TryGetValue(assemblyName, out var cachedAsm))
                    {
                        return cachedAsm;
                    }

                    var executingAssembly = Assembly.GetExecutingAssembly();
                    string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();

                    string targetFileName = assemblyName + ".dll";
                    string resourceName = manifestResourceNames.FirstOrDefault(r =>
                        r.Equals(targetFileName, StringComparison.OrdinalIgnoreCase) ||
                        r.EndsWith("." + targetFileName, StringComparison.OrdinalIgnoreCase));

                    if (resourceName != null)
                    {
                        using (var stream = executingAssembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream != null)
                            {
                                byte[] rawAssembly = new byte[stream.Length];
                                stream.Read(rawAssembly, 0, rawAssembly.Length);
                                var loadedAsm = Assembly.Load(rawAssembly);
                                _resolvedAssemblies[assemblyName] = loadedAsm;
                                return loadedAsm;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Let fallback CLR resolution handle if error occurs
            }

            return null;
        }
    }
}
