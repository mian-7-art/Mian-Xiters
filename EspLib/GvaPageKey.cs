using System;

namespace EspLib
{

public readonly struct GvaPageKey(ulong cr3, ulong va) : IEquatable<GvaPageKey>
{
	public readonly ulong Cr3 = cr3;

	public readonly ulong Va = va;

	public bool Equals(GvaPageKey other)
	{
		if (Cr3 == other.Cr3)
		{
			return Va == other.Va;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is GvaPageKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (int)((Cr3 * 397) ^ Va);
	}
}
}
