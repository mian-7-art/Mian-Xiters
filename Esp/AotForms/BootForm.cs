using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClickableTransparentOverlay;
using Client;

namespace AotForms;

internal sealed class BootForm : Form
{
	private delegate bool EnumChildProc(nint hWnd, nint lParam);

	private struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	private readonly BlueStacks bluestacks = new BlueStacks();

	private static readonly string[] s_emulatorExeNames = new string[6] { "HD-Player", "Bluestacks", "Bluestacks-5", "BstkSVC", "HD-Agent", "HdPlayerService" };

	public BootForm()
	{
		Opacity = 0.0;
		ShowInTaskbar = false;
		FormBorderStyle = FormBorderStyle.None;
		WindowState = FormWindowState.Minimized;
		Shown += OnShown;
	}

	private async void OnShown(object sender, EventArgs e)
	{
		Hide();
		nint viewport = IntPtr.Zero;
		while (viewport == IntPtr.Zero)
		{
			nint num = FindMainWindow();
			if (num != IntPtr.Zero)
			{
				viewport = FindGameViewport(num);
			}
			if (viewport != IntPtr.Zero)
			{
				break;
			}
			await Task.Delay(500);
		}
		Core.Handle = viewport;
		ESP.IsLoading = true;
		ESP eSP = new ESP();
		try
		{
			await ((Overlay)eSP).Start();
		}
		catch
		{
			return;
		}
		Task.Run(async () =>
		{
			while (!(await TryConnectMemory()))
			{
				await Task.Delay(1000);
			}
			ESP.IsLoading = false;
			Thread thread = new Thread(Data.Work);
			thread.IsBackground = true;
			thread.Start();
			Aimbot.Start();
			Thread thread2 = new Thread(Aimmouse.Work);
			thread2.IsBackground = true;
			thread2.Start();
			Thread thread3 = new Thread(() =>
			{
				SilentByPhx.Work();
			});
			thread3.IsBackground = true;
			thread3.Start();
			Thread thread4 = new Thread(UpPlayer.Work);
			thread4.IsBackground = true;
			thread4.Start();
			Thread thread5 = new Thread(Downplayer.Work);
			thread5.IsBackground = true;
			thread5.Start();
		});
	}

	private async Task<bool> TryConnectMemory()
	{
		if (!(await Task.Run(() => bluestacks.Setup("com.dts.freefireth"))))
		{
			return false;
		}
		ulong il2cpp = 0uL;
		for (int i = 0; i < 20; i++)
		{
			il2cpp = bluestacks.GetModuleBase("libil2cpp.so");
			if (il2cpp != 0L)
			{
				break;
			}
			await Task.Delay(500);
		}
		if (il2cpp == 0L)
		{
			return false;
		}
		Offsets.Il2CppBase = (uint)il2cpp;
		Program.PrintRow("PV found", $"0x{Memory.Pvm:X}");
		Thread.Sleep(60);
		Program.PrintRow("VCPU enumerated", $"0x{Memory.Pvcpu:X}");
		Thread.Sleep(60);
		Program.PrintRow("target process located", $"pid {bluestacks.Pid} [{bluestacks.GetTag()}]");
		Thread.Sleep(60);
		Program.PrintRow("CR3 resolved", $"0x{bluestacks.GetCr3():X}");
		Console.WriteLine();
		Thread.Sleep(80);
		Program.PrintReady(bluestacks.GetGameTitle());
		return true;
	}

	private static nint FindMainWindow()
	{
		string[] array = s_emulatorExeNames;
		foreach (string processName in array)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(processName);
				foreach (Process process in processesByName)
				{
					if (process.MainWindowHandle != IntPtr.Zero)
					{
						return process.MainWindowHandle;
					}
				}
			}
			catch
			{
			}
		}
		return IntPtr.Zero;
	}

	[DllImport("user32.dll")]
	private static extern bool EnumChildWindows(nint hWndParent, EnumChildProc lpEnumFunc, nint lParam);

	[DllImport("user32.dll")]
	private static extern bool IsWindowVisible(nint hWnd);

	[DllImport("user32.dll")]
	private static extern bool GetClientRect(nint hWnd, out RECT lpRect);

	[DllImport("user32.dll")]
	private static extern nint FindWindowEx(nint hWndParent, nint hWndChildAfter, string lpszClass, string lpszWindow);

	private static nint FindGameViewport(nint emulatorWnd)
	{
		if (emulatorWnd == IntPtr.Zero)
		{
			return IntPtr.Zero;
		}
		string[] array = new string[4] { "BlueStacksApp", "MSIAppPlayer", "BstkAppPlayer", "BlueStacks" };
		foreach (string lpszClass in array)
		{
			nint num = FindWindowEx(emulatorWnd, IntPtr.Zero, lpszClass, null);
			if (num != IntPtr.Zero && IsWindowVisible(num))
			{
				GetClientRect(num, out var lpRect);
				if (lpRect.Right - lpRect.Left > 100 && lpRect.Bottom - lpRect.Top > 100)
				{
					return num;
				}
			}
		}
		nint bestChild = IntPtr.Zero;
		int bestArea = 0;
		EnumChildWindows(emulatorWnd, (nint hwnd, nint lParam) =>
		{
			if (!IsWindowVisible(hwnd))
			{
				return true;
			}
			GetClientRect(hwnd, out var lpRect2);
			int num2 = (lpRect2.Right - lpRect2.Left) * (lpRect2.Bottom - lpRect2.Top);
			if (num2 > bestArea)
			{
				bestChild = hwnd;
				bestArea = num2;
			}
			return true;
		}, IntPtr.Zero);
		return bestChild;
	}
}
