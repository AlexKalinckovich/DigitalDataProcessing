using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;

public static class AbstractSignalGenerator
{
    public static double[] GenerateThetas(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Frequency frequency,
        Amplitude amplitude,
        double phaseRadians,
        Func<double, double> phaseFunc)
    {
        int samplingRateValue = samplingRate.Value;
        int durationValue = durationInSeconds.Value;
        double frequencyValue = frequency.Value;
        double amplitudeValue = amplitude.Value;

        int totalSamples = durationValue * samplingRateValue;
        double[] signal = new double[totalSamples];

        // Начальная фаза, нормированная к долям периода (фаза в радианах / 2π)
        double phaseOffset = phaseRadians / (2.0 * Math.PI);

        for (int n = 0; n < totalSamples; n++)
        {
            // Нормированная фаза на периоде, θ ∈ [0, 1)
            double theta = (frequencyValue * n / samplingRateValue + phaseOffset) % 1.0;
            theta = theta < 0 ? theta + 1.0 : theta;
            signal[n] = phaseFunc(theta);
        }
        
        return signal;
    }
}