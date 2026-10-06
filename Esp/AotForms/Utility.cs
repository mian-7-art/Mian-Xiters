namespace AotForms;

public static class Utility
{
	public static bool IsLikelyGuestKernelPointer(ulong value)
	{
		return value >= 18446462598732840960uL;
	}
}
