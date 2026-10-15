using System.IO;
using Lab2.App.Src.Main.AudioGeneration.Models;

namespace Lab2.App.Src.Main.AudioGeneration;

public static class WavWriter
{
    public static void Write(string filePath, SamplingRate samplingRate, double[] samples)
    {
        int samplingRateValue = samplingRate.Value;
        short channels = 1;
        short bitsPerSample = 16;
        int blockAlign = channels * (bitsPerSample / 8);
        int dataSize = samples.Length * blockAlign;
        int byteRate = samplingRateValue * blockAlign;

        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var writer = new BinaryWriter(stream);

        writer.Write("RIFF"u8.ToArray());
        writer.Write(36 + dataSize);
        writer.Write("WAVE"u8.ToArray());

        writer.Write("fmt "u8.ToArray());
        writer.Write(16);
        writer.Write((short)1);
        writer.Write(channels);
        writer.Write(samplingRateValue);
        writer.Write(byteRate);
        writer.Write((short)blockAlign);
        writer.Write(bitsPerSample);

        writer.Write("data"u8.ToArray());
        writer.Write(dataSize);

        foreach (double sample in samples)
        {
            short pcm = (short)Math.Max(-32768, Math.Min(32767, sample * 32767));
            writer.Write(pcm);
        }
    }
}