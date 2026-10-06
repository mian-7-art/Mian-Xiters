namespace EspLib
{
    /// <summary>
    /// Checks whether a 64-bit address looks like a guest kernel pointer.
    /// </summary>
    public static class EspUtility
    {
        public static bool IsLikelyGuestKernelPointer(ulong value)
        {
            return value >= 18446462598732840960uL;
        }
    }
}
