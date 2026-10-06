using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AotForms;

internal static class Program
{
	[DllImport("kernel32.dll")]
	private static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	private static extern bool SetConsoleTitle(string lpConsoleTitle);

	internal static void Status(string msg)
	{
	}

	internal static void PrintHeader()
	{
		try
		{
			try
			{
				Console.Clear();
			}
			catch
			{
			}
			SetConsoleTitle("GaySiTo");
			try
			{
				Console.Title = "GaySiTo";
			}
			catch
			{
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("GaySiTo");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("  External");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[+]");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(" Attaching to emulator...");
			Console.WriteLine();
		}
		catch
		{
		}
	}

	internal static void PrintRow(string label, string value)
	{
		try
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(label.PadRight(29));
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("ok");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("  ");
			Console.WriteLine(value);
			Console.ResetColor();
		}
		catch
		{
		}
	}

	internal static void PrintReady(string gameName)
	{
		try
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[+] READY");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("   [" + gameName + "]");
			Console.WriteLine();
			Console.ResetColor();
		}
		catch
		{
		}
	}

	[STAThread]
	private static void Main()
	{
		AiAgentDetector.StartMonitoring();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
		Application.Run(new BootForm());
	}
}
