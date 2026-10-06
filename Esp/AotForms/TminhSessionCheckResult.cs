namespace AotForms;

public enum TminhSessionCheckResult
{
	Active,
	LicenseExpired,
	LicenseRevoked,
	RefreshRequired,
	Retry
}
