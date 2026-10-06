using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AotForms;

public class TminhAuth
{
	private static readonly Lazy<TminhAuth> _instance;

	private readonly object _dataLock = new object();

	private int _requestInProgressInt;

	private int _consecutiveCheckFailures;

	private ulong _lastCheckTick;

	private string _sessionId = "";

	private string _plainSessionKey = "";

	private string _lastNonce = "";

	private static readonly CookieContainer _cookieContainer;

	private static readonly HttpClient _httpClient;

	private static readonly string[] Endpoints;

	public static TminhAuth Instance => _instance.Value;

	public TminhAuthStatus Status { get; private set; }

	public string StatusMessage { get; private set; } = "License verification required.";

	public string VerifiedLicense { get; private set; } = "";

	public string Username { get; private set; } = "";

	public string ProjectName { get; private set; } = "";

	public string AppVersionStr { get; private set; } = "";

	public string AppStatusStr { get; private set; } = "";

	public string DownloadUrlStr { get; private set; } = "";

	public string ExpiresAt { get; private set; } = "";

	public bool HwidLocked { get; private set; }

	public string ServerTime { get; private set; } = "";

	public string AuthToken { get; private set; } = "";

	public bool IsAuthenticated => Status == TminhAuthStatus.Authenticated;

	public bool IsBusy => _requestInProgressInt == 1;

	static TminhAuth()
	{
		_instance = new Lazy<TminhAuth>(() => new TminhAuth());
		Endpoints = new string[3] { "https://gayno1.site.je/api/verify_license.php", "https://gayno1.site.je/Authencation/api/verify_license", "https://tminhauth.duckdns.org/Authencation/api/verify_license" };
		_cookieContainer = new CookieContainer();
		_httpClient = new HttpClient(new HttpClientHandler
		{
			CookieContainer = _cookieContainer,
			ServerCertificateCustomValidationCallback = (HttpRequestMessage sender, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true
		})
		{
			Timeout = TimeSpan.FromSeconds(15.0)
		};
		_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "GAYCorp-GAYAuth/1.2");
		_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
	}

	private TminhAuth()
	{
	}

	private static string AppName()
	{
		return "Mozi h4x";
	}

	private static string OwnerId()
	{
		return "WNaga95C7p";
	}

	private static string AppSecret()
	{
		return "e4ac853fb1c540e8aa442ac21974bef79cc033d3d22a51bf571bd3cda3f78ee9";
	}

	private static string AppVersion()
	{
		return "1.1";
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool GetVolumeInformationW(string lpRootPathName, nint lpVolumeNameBuffer, uint nVolumeNameSize, out uint lpVolumeSerialNumber, nint lpMaximumComponentLength, nint lpFileSystemFlags, nint lpFileSystemNameBuffer, uint nFileSystemNameSize);

	private static string HardwareId()
	{
		try
		{
			string value = "";
			try
			{
				using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Cryptography");
				value = registryKey?.GetValue("MachineGuid")?.ToString() ?? "";
			}
			catch
			{
			}
			uint lpVolumeSerialNumber = 0u;
			try
			{
				GetVolumeInformationW("C:\\", IntPtr.Zero, 0u, out lpVolumeSerialNumber, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0u);
			}
			catch
			{
			}
			string value2 = Environment.MachineName ?? "";
			string s = $"{value}|{lpVolumeSerialNumber}|{value2}";
			return Convert.ToHexString(SHA256.HashData(Encoding.Unicode.GetBytes(s))).ToLowerInvariant();
		}
		catch
		{
			return "DEFAULT_HWID";
		}
	}

	private static string ExecutableHash()
	{
		try
		{
			string text = Environment.ProcessPath;
			if (string.IsNullOrEmpty(text))
			{
				text = Process.GetCurrentProcess().MainModule?.FileName;
			}
			if (string.IsNullOrEmpty(text) || !File.Exists(text))
			{
				return "DEFAULT_HASH";
			}
			using FileStream source = File.Open(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			return Convert.ToHexString(SHA256.HashData(source)).ToLowerInvariant();
		}
		catch
		{
			return "DEFAULT_HASH";
		}
	}

	private static string RandomSeed(int length = 16)
	{
		byte[] array = new byte[length];
		RandomNumberGenerator.Fill(array);
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[array[i] % "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Length]);
		}
		return stringBuilder.ToString();
	}

	private static bool TryBypassInfinityFree(string html, Uri uri)
	{
		try
		{
			MatchCollection matchCollection = Regex.Matches(html, "toNumbers\\(\"([0-9a-fA-F]+)\"\\)");
			if (matchCollection.Count >= 3)
			{
				string value = matchCollection[0].Groups[1].Value;
				string value2 = matchCollection[1].Groups[1].Value;
				string value3 = matchCollection[2].Groups[1].Value;
				using Aes aes = Aes.Create();
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.None;
				aes.Key = Convert.FromHexString(value);
				aes.IV = Convert.FromHexString(value2);
				using ICryptoTransform cryptoTransform = aes.CreateDecryptor();
				byte[] array = Convert.FromHexString(value3);
				string value4 = Convert.ToHexString(cryptoTransform.TransformFinalBlock(array, 0, array.Length)).ToLowerInvariant();
				_cookieContainer.Add(uri, new Cookie("__test", value4, "/"));
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	private static bool SendRequest(Dictionary<string, string> parameters, out string responseBody, out string error)
	{
		responseBody = "";
		error = "";
		string[] endpoints = Endpoints;
		for (int i = 0; i < endpoints.Length; i++)
		{
			Uri uri = new Uri(endpoints[i]);
			for (int j = 0; j < 2; j++)
			{
				try
				{
					using FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
					using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
					{
						Content = content
					};
					httpRequestMessage.Headers.TryAddWithoutValidation("Cache-Control", "no-store");
					HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
					string result = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
					if (httpResponseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(result))
					{
						string text = result.TrimStart();
						if (text.StartsWith("{") || text.StartsWith("["))
						{
							responseBody = result;
							return true;
						}
						if (text.Contains("slowAES.decrypt") && j == 0 && !TryBypassInfinityFree(text, uri))
						{
						}
					}
				}
				catch (Exception ex)
				{
					error = ex.Message;
				}
			}
		}
		if (string.IsNullOrEmpty(error))
		{
			error = "Unable to connect to authentication server.";
		}
		return false;
	}

	private static bool SignedJsonRequest(Dictionary<string, string> parameters, string signingKey, out JsonDocument json, out string error)
	{
		json = null;
		parameters["ownerid"] = OwnerId();
		parameters["name"] = AppName();
		if (!SendRequest(parameters, out var responseBody, out error))
		{
			return false;
		}
		try
		{
			json = JsonDocument.Parse(responseBody);
			return true;
		}
		catch (Exception ex)
		{
			error = "Authentication server returned invalid JSON: " + ex.Message;
			return false;
		}
	}

	public bool InitializeSession(out string error)
	{
		string text = RandomSeed();
		string text2 = AppSecret();
		string value = ExecutableHash();
		if (!SignedJsonRequest(new Dictionary<string, string>
		{
			{ "type", "init" },
			{
				"ver",
				AppVersion()
			},
			{ "hash", value },
			{ "enckey", text }
		}, text2, out var json, out error))
		{
			return false;
		}
		using (json)
		{
			JsonElement rootElement = json.RootElement;
			if (!rootElement.TryGetProperty("success", out var value2) || !value2.GetBoolean())
			{
				error = (rootElement.TryGetProperty("message", out var value3) ? value3.GetString() : "Authentication initialization failed.");
				return false;
			}
			string text3 = (rootElement.TryGetProperty("sessionid", out var value4) ? value4.GetString() : "");
			string lastNonce = (rootElement.TryGetProperty("nonce", out var value5) ? value5.GetString() : "");
			if (string.IsNullOrEmpty(text3))
			{
				error = "Authentication response is incomplete.";
				return false;
			}
			lock (_dataLock)
			{
				_sessionId = text3;
				_plainSessionKey = text + "-" + text2;
				_lastNonce = lastNonce;
			}
			return true;
		}
	}

	public bool AuthenticateLicense(string license, out string error, out bool licenseRejected)
	{
		licenseRejected = false;
		string sessionId;
		string plainSessionKey;
		lock (_dataLock)
		{
			sessionId = _sessionId;
			plainSessionKey = _plainSessionKey;
		}
		string value = HardwareId();
		if (!SignedJsonRequest(new Dictionary<string, string>
		{
			{ "type", "license" },
			{ "sessionid", sessionId },
			{ "key", license },
			{ "hwid", value }
		}, plainSessionKey, out var json, out error))
		{
			return false;
		}
		bool result;
		using (json)
		{
			JsonElement rootElement = json.RootElement;
			if (!rootElement.TryGetProperty("success", out var value2) || !value2.GetBoolean())
			{
				error = (rootElement.TryGetProperty("message", out var value3) ? value3.GetString() : "Invalid license key.");
				licenseRejected = true;
				result = false;
			}
			else
			{
				lock (_dataLock)
				{
					if (rootElement.TryGetProperty("info", out var value4))
					{
						Username = (value4.TryGetProperty("username", out var value5) ? value5.GetString() : "");
					}
					if (rootElement.TryGetProperty("data", out var value6))
					{
						ProjectName = (value6.TryGetProperty("project_name", out var value7) ? value7.GetString() : "N/A");
						AppVersionStr = (value6.TryGetProperty("app_version", out var value8) ? value8.GetString() : "1.0.0");
						AppStatusStr = (value6.TryGetProperty("app_status", out var value9) ? value9.GetString() : "online");
						DownloadUrlStr = (value6.TryGetProperty("download_url", out var value10) ? value10.GetString() : "");
						ExpiresAt = (value6.TryGetProperty("expires_at", out var value11) ? value11.GetString() : "LIFETIME");
						HwidLocked = value6.TryGetProperty("hwid_locked", out var value12) && value12.GetBoolean();
						ServerTime = (value6.TryGetProperty("server_time", out var value13) ? value13.GetString() : "");
						AuthToken = (value6.TryGetProperty("auth_token", out var value14) ? value14.GetString() : "");
					}
					VerifiedLicense = license;
					StatusMessage = "License verified.";
				}
				result = true;
			}
		}
		return result;
	}

	public bool VerifyLicenseSync(string license, out string outError)
	{
		if (string.IsNullOrEmpty(license) || license.Length < 3)
		{
			outError = "License key too short.";
			return false;
		}
		Status = TminhAuthStatus.Verifying;
		SetMessage("Verifying license...");
		bool licenseRejected = false;
		if (InitializeSession(out outError) && AuthenticateLicense(license, out outError, out licenseRejected))
		{
			lock (_dataLock)
			{
				VerifiedLicense = license;
				StatusMessage = "License verified.";
			}
			Status = TminhAuthStatus.Authenticated;
			return true;
		}
		SetFailure(string.IsNullOrEmpty(outError) ? "License verification failed." : outError);
		return false;
	}

	public void VerifyLicenseAsync(string license, Action<bool, string> onComplete = null)
	{
		Task.Run(() =>
		{
			bool arg = VerifyLicenseSync(license, out var outError);
			onComplete?.Invoke(arg, outError);
		});
	}

	public TminhSessionCheckResult CheckSession(out string error)
	{
		string sessionId;
		string plainSessionKey;
		lock (_dataLock)
		{
			sessionId = _sessionId;
			plainSessionKey = _plainSessionKey;
		}
		if (!SignedJsonRequest(new Dictionary<string, string>
		{
			{ "type", "check" },
			{ "sessionid", sessionId }
		}, plainSessionKey, out var json, out error))
		{
			return TminhSessionCheckResult.Retry;
		}
		using (json)
		{
			JsonElement rootElement = json.RootElement;
			if (!rootElement.TryGetProperty("success", out var value) || !value.GetBoolean())
			{
				error = (rootElement.TryGetProperty("message", out var value2) ? value2.GetString() : "Session expired.");
				return TminhSessionCheckResult.RefreshRequired;
			}
			return TminhSessionCheckResult.Active;
		}
	}

	public bool RefreshAuthenticatedSession(out string error, out bool licenseRejected)
	{
		licenseRejected = false;
		string verifiedLicense;
		lock (_dataLock)
		{
			verifiedLicense = VerifiedLicense;
		}
		if (string.IsNullOrEmpty(verifiedLicense))
		{
			error = "License is unavailable.";
			licenseRejected = true;
			return false;
		}
		if (!InitializeSession(out error))
		{
			return false;
		}
		return AuthenticateLicense(verifiedLicense, out error, out licenseRejected);
	}

	public void Tick()
	{
		if (Status != TminhAuthStatus.Authenticated)
		{
			return;
		}
		ulong tickCount = (ulong)Environment.TickCount64;
		if (tickCount - _lastCheckTick < 10000 || Interlocked.CompareExchange(ref _requestInProgressInt, 1, 0) != 0)
		{
			return;
		}
		_lastCheckTick = tickCount;
		Task.Run(() =>
		{
			try
			{
				TminhSessionCheckResult tminhSessionCheckResult = CheckSession(out var error);
				switch (tminhSessionCheckResult)
				{
				case TminhSessionCheckResult.Active:
					_consecutiveCheckFailures = 0;
					SetMessage("License verified.");
					break;
				case TminhSessionCheckResult.LicenseExpired:
					SetFailure("License expired.");
					break;
				case TminhSessionCheckResult.LicenseRevoked:
					SetFailure(string.IsNullOrEmpty(error) ? "License revoked." : error);
					break;
				default:
				{
					int num = Interlocked.Increment(ref _consecutiveCheckFailures);
					if (tminhSessionCheckResult == TminhSessionCheckResult.RefreshRequired || num >= 3)
					{
						if (RefreshAuthenticatedSession(out var error2, out var licenseRejected))
						{
							_consecutiveCheckFailures = 0;
							_lastCheckTick = (ulong)Environment.TickCount64;
							SetMessage("License verified.");
						}
						else if (licenseRejected)
						{
							SetFailure(string.IsNullOrEmpty(error2) ? "License expired." : error2);
						}
						else
						{
							SetMessage("Connection interrupted. Retrying...");
						}
					}
					else
					{
						SetMessage("Connection interrupted. Retrying...");
					}
					break;
				}
				}
			}
			finally
			{
				Interlocked.Exchange(ref _requestInProgressInt, 0);
			}
		});
	}

	private void SetMessage(string message)
	{
		lock (_dataLock)
		{
			StatusMessage = message;
		}
	}

	private void SetFailure(string message)
	{
		lock (_dataLock)
		{
			StatusMessage = message;
			VerifiedLicense = "";
			_sessionId = "";
			_plainSessionKey = "";
			_lastNonce = "";
		}
		Status = TminhAuthStatus.Failed;
	}

	public string GetStatusMessage()
	{
		lock (_dataLock)
		{
			return StatusMessage;
		}
	}
}
