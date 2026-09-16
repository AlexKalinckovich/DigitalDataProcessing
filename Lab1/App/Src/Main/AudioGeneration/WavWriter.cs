using System.Text;
using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;

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

        writer.Write(Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(36 + dataSize);
        writer.Write(Encoding.ASCII.GetBytes("WAVE"));

        writer.Write(Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16);
        writer.Write((short)1);
        writer.Write(channels);
        writer.Write(samplingRateValue);
        writer.Write(byteRate);
        writer.Write((short)blockAlign);
        writer.Write(bitsPerSample);

        writer.Write(Encoding.ASCII.GetBytes("data"));
        writer.Write(dataSize);

        foreach (double sample in samples)
        {
            short pcm = (short)Math.Max(-32768, Math.Min(32767, sample * 32767));
            writer.Write(pcm);
        }
    }
}