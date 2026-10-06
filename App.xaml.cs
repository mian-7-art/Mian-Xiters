using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += (sender, args) =>
            {
                DiagnosticLogger.LogCrash(args.Exception, "WPF UI Dispatcher Exception");
                args.Handled = true;
            };
        }
    }
}
