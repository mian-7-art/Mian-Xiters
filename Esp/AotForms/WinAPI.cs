using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AotForms;

internal static class WinAPI
{
	public struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	public delegate bool EnumWindowsProc(nint hWnd, nint lParam);

	internal const int WM_NCLBUTTONDOWN = 161;

	internal const int HT_CAPTION = 2;

	[DllImport("user32.dll")]
	public static extern bool GetWindowRect(nint hwnd, out RECT rect);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern nint FindWindow(string lpClassName, string lpWindowName);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, nint lParam);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool EnumChildWindows(nint hWndParent, EnumWindowsProc lpEnumFunc, nint lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetWindowText(nint hWnd, StringBuilder lpString, int nMaxCount);

	[DllImport("user32.dll")]
	public static extern short GetAsyncKeyState(Keys vKey);

	[DllImport("Gdi32.dll")]
	internal static extern nint CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	[DllImport("user32.dll")]
	internal static extern int SendMessage(nint hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	internal static extern bool ReleaseCapture();
}
