using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Прямоугольный (импульсный) сигнал: биполярный, переключается между +A и -A.
/// </summary>
public static class PulseSignal
{
    /// <param name="phaseRadians">Начальная фаза — сдвиг волны по времени в радианах.</param>
    /// <param name="dutyRatio">
    /// Скважность — отношение периода повторения к длительности импульса
    /// (например, 2 для меандра).
    /// </param>
    public static double[] Generate(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Frequency frequency,
        Amplitude amplitude,
        DutyRatio dutyRatio,
        double phaseRadians)
    {
        int samplingRateValue = samplingRate.Value;
        int durationValue = durationInSeconds.Value;
        double frequencyValue = frequency.Value;
        double amplitudeValue = amplitude.Value;
        double dutyRatioValue = dutyRatio.Value;
        
        int totalSamples = durationValue * samplingRateValue;
        double[] signal = new double[totalSamples];

        // Коэффициент заполнения: доля периода, в которой сигнал равен +A
        double dutyCycle = 1.0 / dutyRatioValue;

        // Начальная фаза, нормированная к долям периода (фаза в радианах / 2π)
        double phaseOffset = phaseRadians / (2.0 * Math.PI);

        for (int n = 0; n < totalSamples; n++)
        {
            // Нормированная фаза на периоде, θ ∈ [0, 1)
            double theta = (frequencyValue * n / samplingRateValue + phaseOffset) % 1.0;
            theta = theta < 0 ? theta + 1.0 : theta;
            signal[n] = (theta < dutyCycle) ? amplitudeValue : -amplitudeValue;
        }
        return signal;
    }
}