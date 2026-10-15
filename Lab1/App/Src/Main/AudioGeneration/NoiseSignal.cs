using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;


public static class NoiseSignal
{
    public static double[] GenerateUniformNoise(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Amplitude amplitude)
    {
        int samplingRateValue = samplingRate.Value;
        int durationValue = durationInSeconds.Value;
        double amplitudeValue = amplitude.Value;

        int totalSamples = durationValue * samplingRateValue;
        double[] signal = new double[totalSamples];
        var random = Random.Shared;

        for (int n = 0; n < totalSamples; n++)
        {
            
            double u = random.NextDouble();
            signal[n] = amplitudeValue * (2.0 * u - 1.0);
        }

        return signal;
    }
    public static double[] GenerateGaussianNoise(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Amplitude amplitude)
    {
        int samplingRateValue = samplingRate.Value;
        int durationValue = durationInSeconds.Value;
        double amplitudeValue = amplitude.Value;

        int totalSamples = durationValue * samplingRateValue;
        double[] signal = new double[totalSamples];
        var random = Random.Shared;

        double sigma = amplitudeValue / 3.0;
        double twoPi = 2.0 * Math.PI;

        for (int n = 0; n < totalSamples; n++)
        {
            
            double u1 = 1.0 - random.NextDouble();
            double u2 = random.NextDouble();

            
            double zn = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(twoPi * u2);
            signal[n] = sigma * zn;
        }

        return signal;
    }
}