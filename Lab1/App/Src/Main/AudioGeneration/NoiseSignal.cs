
using System;

namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Предоставляет методы для генерации стохастических (шумовых) сигналов.
/// В отличие от детерминированных волн, значения шума предсказуемы только 
/// в рамках статистических характеристик (равномерное или гауссовское распределение).
/// </summary>
public static class NoiseSignal
{
    /// <summary>
    /// Генерирует белый шум с равномерным распределением амплитуд.
    /// Каждый отсчет является независимой случайной величиной в диапазоне $[-A, +A]$.
    /// </summary>
    /// <param name="samplingRate">
    /// Частота дискретизации ($f_d$) — количество отсчетов (чисел) в секунду.
    /// </param>
    /// <param name="durationSeconds">
    /// Длительность сигнала ($t_{\text{dur}}$) — общее время звучания в секундах.
    /// </param>
    /// <param name="amplitude">
    /// Амплитуда ($A$) — пиковая высота сигнала (диапазон от $0.0$ до $1.0$).
    /// </param>
    /// <returns>
    /// Массив вещественных чисел (<c>double[]</c>), представляющий равномерный белый шум.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если параметры выходят за физические пределы 
    /// (например, амплитуда вне диапазона $[0.0, 1.0]$ или отрицательная длительность).
    /// </exception>
    public static double[] GenerateUniformNoise(
        double samplingRate,
        double durationSeconds,
        double amplitude)
    {
        if (samplingRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(samplingRate), "Частота дискретизации должна быть больше нуля.");
        if (durationSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Длительность сигнала должна быть больше нуля.");
        if (amplitude < 0.0 || amplitude > 1.0)
            throw new ArgumentOutOfRangeException(nameof(amplitude), "Амплитуда должна находиться в диапазоне от 0.0 до 1.0.");

        int totalSamples = (int)(durationSeconds * samplingRate);
        double[] signal = new double[totalSamples];
        var random = Random.Shared; // Потокобезопасный генератор (доступен в .NET 6+)

        for (int n = 0; n < totalSamples; n++)
        {
            // U_n в полуинтервале [0.0, 1.0)
            double u = random.NextDouble();
                
            // Линейное масштабирование в диапазон [-A, +A]
            signal[n] = amplitude * (2.0 * u - 1.0);
        }

        return signal;
    }

    /// <summary>
    /// Генерирует белый шум с гауссовским (нормальным) распределением амплитуд.
    /// Использует преобразование Бокса — Мюллера и правило трех сигм ($\sigma = A / 3.0$).
    /// </summary>
    /// <param name="samplingRate">
    /// Частота дискретизации ($f_d$) — количество отсчетов (чисел) в секунду.
    /// </param>
    /// <param name="durationSeconds">
    /// Длительность сигнала ($t_{\text{dur}}$) — общее время звучания в секундах.
    /// </param>
    /// <param name="amplitude">
    /// Амплитуда ($A$) — целевой предел, в который укладывается 99.7% значений ($\pm 3\sigma$).
    /// </param>
    /// <returns>
    /// Массив вещественных чисел (<c>double[]</c>), представляющий гауссовский белый шум.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если параметры выходят за физические пределы 
    /// (например, амплитуда вне диапазона $[0.0, 1.0]$ или отрицательная длительность).
    /// </exception>
    public static double[] GenerateGaussianNoise(
        double samplingRate,
        double durationSeconds,
        double amplitude)
    {
        if (samplingRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(samplingRate), "Частота дискретизации должна быть больше нуля.");
        if (durationSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Длительность сигнала должна быть больше нуля.");
        if (amplitude < 0.0 || amplitude > 1.0)
            throw new ArgumentOutOfRangeException(nameof(amplitude), "Амплитуда должна находиться в диапазоне от 0.0 до 1.0.");

        int totalSamples = (int)(durationSeconds * samplingRate);
        double[] signal = new double[totalSamples];
        var random = Random.Shared;
            
        // Правило трех сигм: sigma = A / 3.0
        double sigma = amplitude / 3.0;
        double twoPi = 2.0 * Math.PI;

        for (int n = 0; n < totalSamples; n++)
        {
            // U1 строго в интервале (0.0, 1.0], чтобы избежать Math.Log(0)
            double u1 = 1.0 - random.NextDouble();
                
            // U2 в интервале [0.0, 1.0)
            double u2 = random.NextDouble();

            // Преобразование Бокса — Мюллера для получения стандартной нормальной величины z_n
            double zn = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(twoPi * u2);

            // Масштабирование по правилу трех сигм
            signal[n] = sigma * zn;
        }

        return signal;
    }
}