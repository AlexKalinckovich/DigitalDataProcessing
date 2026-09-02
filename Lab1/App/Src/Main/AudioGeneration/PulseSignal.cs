namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Прямоугольный (импульсный) сигнал: биполярный, переключается между +A и -A.
/// </summary>
public class PulseSignal
{
    private readonly SamplingRate _samplingRate;
    private readonly Duration _duration;
    private readonly Frequency _frequency;
    private readonly Amplitude _amplitude;

    // Начальная фаза, нормированная к долям периода (фаза в радианах / 2π)
    private readonly double _phaseOffset;

    // Коэффициент заполнения: доля периода, в которой сигнал равен +A
    private readonly double _dutyCycle;

    /// <param name="phaseRadians">Начальная фаза — сдвиг волны по времени в радианах.</param>
    /// <param name="dutyRatio">
    /// Скважность — отношение периода повторения к длительности импульса
    /// (например, 2 для меандра).
    /// </param>
    public PulseSignal(
        SamplingRate samplingRate,
        Duration duration,
        Frequency frequency,
        Amplitude amplitude,
        double phaseRadians,
        double dutyRatio)
    {
        if (dutyRatio < 1.0)
            throw new ArgumentOutOfRangeException(nameof(dutyRatio), "Скважность не может быть меньше 1.0.");

        _samplingRate = samplingRate;
        _duration = duration;
        _frequency = frequency;
        _amplitude = amplitude;
        _phaseOffset = phaseRadians / (2.0 * Math.PI);
        _dutyCycle = 1.0 / dutyRatio;
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

            signal[n] = (theta < _dutyCycle) ? _amplitude.Value : -_amplitude.Value;
        }

        return signal;
    }
}