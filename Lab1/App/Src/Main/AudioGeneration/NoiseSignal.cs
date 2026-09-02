namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Шумовые (стохастические) сигналы. Значения предсказуемы только
/// в рамках статистических характеристик распределения.
/// </summary>
public static class NoiseSignal
{
    /// <summary>Белый шум с равномерным распределением амплитуд в диапазоне [-A, +A].</summary>
    public static double[] GenerateUniformNoise(
        SamplingRate samplingRate,
        Duration duration,
        Amplitude amplitude)
    {
        int totalSamples = duration.Value * samplingRate.Value;
        double[] signal = new double[totalSamples];
        var random = Random.Shared;

        for (int n = 0; n < totalSamples; n++)
        {
            // U ∈ [0, 1), линейно масштабируем в [-A, +A]
            double u = random.NextDouble();
            signal[n] = amplitude.Value * (2.0 * u - 1.0);
        }

        return signal;
    }

    /// <summary>
    /// Белый шум с гауссовским распределением.
    /// Преобразование Бокса — Мюллера и правило трёх сигм (σ = A / 3),
    /// поэтому 99.7% значений укладываются в [-A, +A].
    /// </summary>
    public static double[] GenerateGaussianNoise(
        SamplingRate samplingRate,
        Duration duration,
        Amplitude amplitude)
    {
        int totalSamples = duration.Value * samplingRate.Value;
        double[] signal = new double[totalSamples];
        var random = Random.Shared;

        double sigma = amplitude.Value / 3.0;
        double twoPi = 2.0 * Math.PI;

        for (int n = 0; n < totalSamples; n++)
        {
            // U1 ∈ (0, 1] — строго положительное, чтобы избежать Math.Log(0)
            double u1 = 1.0 - random.NextDouble();
            double u2 = random.NextDouble();

            // Преобразование Бокса — Мюллера для стандартной нормальной величины
            double zn = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(twoPi * u2);
            signal[n] = sigma * zn;
        }

        return signal;
    }
}