using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1
{
    public static class DiscordRPCManager
    {
        private const string CLIENT_ID = "1551562985535701022";
        private const string LARGE_IMAGE_KEY = "logo";
        private const string LARGE_IMAGE_TOOLTIP = "MIAN XITERS • Dev: @mian_7";
        private const string SMALL_IMAGE_KEY = "logo";
        private const string SMALL_IMAGE_TOOLTIP = "Status: Safe & Undetected";
        
        private const string BUTTON1_LABEL = "Contact Developer";
        private const string BUTTON1_URL = "https://discord.com/users/1177824268847943702";
        
        private const string BUTTON2_LABEL = "Buy Here";
        private const string BUTTON2_URL = "https://discord.com/users/1177824268847943702";

        private static NamedPipeClientStream _pipeClient;
        private static readonly object _lock = new object();
        private static bool _isConnected = false;
        private static long _startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        private static string _currentUsername = "";
        private static int _currentDaysLeft = 0;
        private static bool _isLoggedIn = false;
        private static CancellationTokenSource _cts;

        public static void InitDiscordRPC()
        {
            _cts = new CancellationTokenSource();
            _startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Task.Run(async () =>
            {
                while (_cts != null && !_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        if (!_isConnected)
                        {
                            EnsureConnected();
                        }
                    }
                    catch { }

                    await Task.Delay(4000);
                }
            });
        }

        private static bool EnsureConnected()
        {
            lock (_lock)
            {
                if (_isConnected && _pipeClient != null && _pipeClient.IsConnected)
                    return true;

                ClosePipe();

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        var pipe = new NamedPipeClientStream(".", $"discord-ipc-{i}", PipeDirection.InOut, PipeOptions.Asynchronous);
                        pipe.Connect(500);

                        if (pipe.IsConnected)
                        {
                            _pipeClient = pipe;

                            // Send Handshake (Opcode 0)
                            var handshake = new { v = 1, client_id = CLIENT_ID };
                            SendFrame(0, JsonConvert.SerializeObject(handshake));

                            // Read Handshake response
                            byte[] header = new byte[8];
                            int read = _pipeClient.Read(header, 0, 8);
                            if (read == 8)
                            {
                                int length = BitConverter.ToInt32(header, 4);
                                byte[] body = new byte[length];
                                _pipeClient.Read(body, 0, length);
                            }

                            _isConnected = true;

                            // Send presence immediately upon connection
                            SendPresenceInternal();

                            return true;
                        }
                    }
                    catch
                    {
                        ClosePipe();
                    }
                }

                return false;
            }
        }

        private static string _activeFeature = "";

        public static void UpdateActiveFeature(string featureName)
        {
            _activeFeature = string.IsNullOrWhiteSpace(featureName) ? "" : featureName;
            Task.Run(() =>
            {
                lock (_lock)
                {
                    if (EnsureConnected())
                    {
                        SendPresenceInternal();
                    }
                }
            });
        }

        public static void UpdateDiscordPresence(string username, int daysLeft)
        {
            _currentUsername = string.IsNullOrWhiteSpace(username) ? "User" : username;
            _currentDaysLeft = daysLeft;
            _isLoggedIn = true;

            Task.Run(() =>
            {
                lock (_lock)
                {
                    if (EnsureConnected())
                    {
                        SendPresenceInternal();
                    }
                }
            });
        }

        public static void UpdateDiscordPresenceFromAuth(AuthlyX.Auth auth)
        {
            try
            {
                string username = auth?.userData?.Username;
                int daysLeft = 0;

                if (auth?.userData != null)
                {
                    daysLeft = auth.userData.DaysLeft;
                    if (daysLeft <= 0 && !string.IsNullOrEmpty(auth.userData.ExpiryDate))
                    {
                        if (DateTime.TryParse(auth.userData.ExpiryDate, out DateTime exp))
                        {
                            TimeSpan span = exp - DateTime.Now;
                            daysLeft = span.Days > 0 ? span.Days : 0;
                        }
                        else if (long.TryParse(auth.userData.ExpiryDate, out long epoch))
                        {
                            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            daysLeft = (epoch > now) ? (int)((epoch - now) / 86400) : 0;
                        }
                    }
                }

                UpdateDiscordPresence(username, daysLeft);
            }
            catch
            {
                UpdateDiscordPresence(auth?.userData?.Username ?? "User", 0);
            }
        }

        private static void SendPresenceInternal()
        {
            try
            {
                string details = _isLoggedIn ? $"User: {_currentUsername} [VIP]" : "MIAN XITERS Premium";
                string featState = _activeFeature;
                if (!string.IsNullOrEmpty(featState))
                {
                    if (featState.EndsWith(" ON", StringComparison.OrdinalIgnoreCase))
                        featState = featState.Substring(0, featState.Length - 3).Trim();
                    featState = $"{featState.ToUpper()} ENABLED !";
                }
                else
                {
                    featState = _isLoggedIn ? $"License: {_currentDaysLeft} Days Left" : "Status: Undetected";
                }
                string state = featState;

                var payload = new
                {
                    cmd = "SET_ACTIVITY",
                    args = new
                    {
                        pid = Process.GetCurrentProcess().Id,
                        activity = new
                        {
                            state = state,
                            details = details,
                            timestamps = new
                            {
                                start = _startTimestamp
                            },
                            assets = new
                            {
                                large_image = LARGE_IMAGE_KEY,
                                large_text = LARGE_IMAGE_TOOLTIP,
                                small_image = SMALL_IMAGE_KEY,
                                small_text = SMALL_IMAGE_TOOLTIP
                            },
                            buttons = new[]
                            {
                                new { label = BUTTON1_LABEL, url = BUTTON1_URL },
                                new { label = BUTTON2_LABEL, url = BUTTON2_URL }
                            }
                        }
                    },
                    nonce = Guid.NewGuid().ToString()
                };

                SendFrame(1, JsonConvert.SerializeObject(payload));
            }
            catch
            {
                ClosePipe();
            }
        }

        private static void SendFrame(int opcode, string json)
        {
            if (_pipeClient == null || !_pipeClient.IsConnected) return;

            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            byte[] opcodeBytes = BitConverter.GetBytes(opcode);
            byte[] lengthBytes = BitConverter.GetBytes(jsonBytes.Length);

            byte[] buffer = new byte[8 + jsonBytes.Length];
            Buffer.BlockCopy(opcodeBytes, 0, buffer, 0, 4);
            Buffer.BlockCopy(lengthBytes, 0, buffer, 4, 4);
            Buffer.BlockCopy(jsonBytes, 0, buffer, 8, jsonBytes.Length);

            _pipeClient.Write(buffer, 0, buffer.Length);
            _pipeClient.Flush();
        }

        private static void ClosePipe()
        {
            try
            {
                _isConnected = false;
                _pipeClient?.Dispose();
                _pipeClient = null;
            }
            catch { }
        }

        public static void CleanDiscordRPC()
        {
            try
            {
                _cts?.Cancel();
                lock (_lock)
                {
                    if (_isConnected && _pipeClient != null && _pipeClient.IsConnected)
                    {
                        var payload = new
                        {
                            cmd = "SET_ACTIVITY",
                            args = new
                            {
                                pid = Process.GetCurrentProcess().Id,
                                activity = (object)null
                            },
                            nonce = Guid.NewGuid().ToString()
                        };
                        SendFrame(1, JsonConvert.SerializeObject(payload));
                    }
                    ClosePipe();
                }
            }
            catch { }
        }
    }
}
