using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Assembly resolution is handled by Program.cs (custom entry point)
        // which hooks AssemblyResolve BEFORE any WPF types are loaded.
        // This guarantees the single-exe deployment works without companion DLLs.
    }
}
