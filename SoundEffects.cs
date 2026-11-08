using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class SoundEffects
    {
        public static volatile bool IsMuted = false;
        public static void PlayEnableSound()
        {
            if (IsMuted) return;
            Task.Run(() =>
            {
                try
                {
                    byte[] wav = GenerateCyberTone(freqStart: 520, freqEnd: 880, durationMs: 120, volume: 0.25f);
                    using (var ms = new MemoryStream(wav))
                    {
                        using (var sp = new SoundPlayer(ms))
                        {
                            sp.PlaySync();
                        }
                    }
                }
                catch { }
            });
        }

        public static void PlayDisableSound()
        {
            if (IsMuted) return;
            Task.Run(() =>
            {
                try
                {
                    byte[] wav = GenerateCyberTone(freqStart: 640, freqEnd: 360, durationMs: 100, volume: 0.20f);
                    using (var ms = new MemoryStream(wav))
                    using (var sp = new SoundPlayer(ms))
                        sp.PlaySync();
                }
                catch { }
            });
        }

        // Neutral ping — played when a scan begins (neither positive nor negative)
        public static void PlayScanningSound()
        {
            Task.Run(() =>
            {
                try
                {
                    // Flat mid-range tone: 700Hz steady, 70ms — short, subtle, informational
                    byte[] wav = GenerateCyberTone(freqStart: 700, freqEnd: 700, durationMs: 70, volume: 0.15f);
                    using (var ms = new MemoryStream(wav))
                    using (var sp = new SoundPlayer(ms))
                        sp.PlaySync();
                }
                catch { }
            });
        }

        // Error beep — played on Pattern Not Found, Emulator Not Found, injection failure
        public static void PlayErrorSound()
        {
            Task.Run(() =>
            {
                try
                {
                    // First buzz: sharp high-to-low
                    byte[] buzz1 = GenerateCyberTone(freqStart: 520, freqEnd: 180, durationMs: 110, volume: 0.28f);
                    // Brief gap then second buzz: lower harsh drop
                    byte[] buzz2 = GenerateCyberTone(freqStart: 300, freqEnd: 140, durationMs: 110, volume: 0.22f);

                    using (var ms1 = new MemoryStream(buzz1))
                    using (var sp1 = new SoundPlayer(ms1))
                        sp1.PlaySync();

                    System.Threading.Thread.Sleep(40);

                    using (var ms2 = new MemoryStream(buzz2))
                    using (var sp2 = new SoundPlayer(ms2))
                        sp2.PlaySync();
                }
                catch { }
            });
        }

        private static byte[] GenerateCyberTone(double freqStart, double freqEnd, int durationMs, float volume)
        {
            int sampleRate = 44100;
            short bitsPerSample = 16;
            short channels = 1;

            int numSamples = (int)(sampleRate * (durationMs / 1000.0));
            int dataSize = numSamples * (bitsPerSample / 8);

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // RIFF header
                bw.Write(new char[] { 'R', 'I', 'F', 'F' });
                bw.Write(36 + dataSize);
                bw.Write(new char[] { 'W', 'A', 'V', 'E' });

                // fmt chunk
                bw.Write(new char[] { 'f', 'm', 't', ' ' });
                bw.Write(16); // SubChunk1Size
                bw.Write((short)1); // AudioFormat PCM
                bw.Write(channels);
                bw.Write(sampleRate);
                bw.Write(sampleRate * channels * (bitsPerSample / 8)); // ByteRate
                bw.Write((short)(channels * (bitsPerSample / 8))); // BlockAlign
                bw.Write(bitsPerSample);

                // data chunk
                bw.Write(new char[] { 'd', 'a', 't', 'a' });
                bw.Write(dataSize);

                double currentPhase = 0;
                for (int i = 0; i < numSamples; i++)
                {
                    double t = (double)i / numSamples; // 0.0 to 1.0
                    double currentFreq = freqStart + (freqEnd - freqStart) * t;

                    currentPhase += 2.0 * Math.PI * currentFreq / sampleRate;

                    // Envelope: Fade in 10%, Fade out 30%
                    double envelope = 1.0;
                    if (t < 0.1) envelope = t / 0.1;
                    else if (t > 0.7) envelope = (1.0 - t) / 0.3;

                    short sampleVal = (short)(Math.Sin(currentPhase) * short.MaxValue * volume * envelope);
                    bw.Write(sampleVal);
                }

                bw.Flush();
                return ms.ToArray();
            }
        }
    }
}
