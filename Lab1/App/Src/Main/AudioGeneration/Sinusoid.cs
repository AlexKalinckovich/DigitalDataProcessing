namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Синусоидальный сигнал: s(t) = A · sin(2πF·t + φ).
/// </summary>
public class Sinusoid
{
    private readonly SamplingRate _samplingRate;
    private readonly Duration _duration;
    private readonly Frequency _frequency;
    private readonly Amplitude _amplitude;

    // Начальная фаза, нормированная к долям периода (фаза в радианах / 2π)
    private readonly double _phaseOffset;

    /// <param name="phaseRadians">Начальная фаза — сдвиг волны по времени в радианах.</param>
    public Sinusoid(
        SamplingRate samplingRate,
        Duration duration,
        Frequency frequency,
        Amplitude amplitude,
        double phaseRadians = 0.0)
    {
        _samplingRate = samplingRate;
        _duration = duration;
        _frequency = frequency;
        _amplitude = amplitude;
        _phaseOffset = phaseRadians / (2.0 * Math.PI);
    }

    /// <summary>Генерирует массив отсчётов сигнала.</summary>
    public double[] Generate()
    {
        int totalSamples = _duration.Value * _samplingRate.Value;
        double[] signal = new double[totalSamples];

        for (int n = 0; n < totalSamples; n++)
        {
            // Нормированная фаза на периоде, θ ∈ [0, 1)
            double theta = (_frequency.Value * n / _samplingRate.Value + _phaseOffset) % 1.0;
            // Остаток от деления в C# может быть отрицательным
            if (theta < 0) theta += 1.0;

            signal[n] = _amplitude.Value * Math.Sin(2.0 * Math.PI * theta);
        }

        return signal;
    }
}