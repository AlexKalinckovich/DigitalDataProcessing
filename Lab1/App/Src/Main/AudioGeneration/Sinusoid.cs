using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Синусоидальный сигнал: s(t) = A · sin(2πF·t + φ).
/// </summary>
public static class Sinusoid
{
    /// <param name="phaseRadians">Начальная фаза — сдвиг волны по времени в радианах.</param>
    public static double[] Generate(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Frequency frequency,
        Amplitude amplitude,
        double phaseRadians = 0.0)
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
            double theta = (frequencyValue * n / samplingRateValue + phaseOffset) % 1.0;
            theta = theta < 0 ? theta + 1.0 : theta;
            signal[n] = amplitudeValue * Math.Sin(2.0 * Math.PI * theta);
        }
        
        return signal;
    }
}