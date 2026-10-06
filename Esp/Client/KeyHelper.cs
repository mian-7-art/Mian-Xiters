using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Client;

internal class KeyHelper
{
	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(Keys vKey);

	public static bool IsKeyDown(Keys key)
	{
		if (key == Keys.None)
		{
			return false;
		}
		return (GetAsyncKeyState(key) & 0x8000) != 0;
	}
}
