

namespace  Lab1.App.Src.Main.AudioGeneration;
using System;

/// <summary>
/// Предоставляет методы для генерации цифровых пилообразных сигналов.
/// Пилообразная волна линейно нарастает и мгновенно сбрасывается, 
/// что создает резкий, насыщенный гармониками тембр.
/// </summary>
public static class SawtoothSignal
{
    /// <summary>
    /// Генерирует массив отсчетов, описывающий пилообразный сигнал.
    /// </summary>
    /// <param name="samplingRate">
    /// Частота дискретизации ($f_d$) — количество отсчетов (чисел) в секунду.
    /// </param>
    /// <param name="durationSeconds">
    /// Длительность сигнала ($t_{\text{dur}}$) — общее время звучания в секундах.
    /// </param>
    /// <param name="frequencyHz">
    /// Частота сигнала ($F$) — количество полных циклов (зубьев) в секунду (в Гц).
    /// </param>
    /// <param name="amplitude">
    /// Амплитуда ($A$) — пиковая высота сигнала (диапазон от $0.0$ до $1.0$).
    /// </param>
    /// <param name="phaseRadians">
    /// Начальная фаза ($\phi$) — начальный сдвиг сигнала по оси времени (в радианах).
    /// </param>
    /// <returns>
    /// Массив вещественных чисел (<c>double[]</c>), представляющий цифровой пилообразный сигнал.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если параметры выходят за физические пределы 
    /// (например, амплитуда вне диапазона $[0.0, 1.0]$ или отрицательная длительность).
    /// </exception>
    /// <example>
    /// <code>
    /// // Генерация 1-секундного пилообразного сигнала частотой 440 Гц
    /// double[] sawtoothData = SawtoothSignal.Generate(
    ///     samplingRate: 44100,
    ///     durationSeconds: 1.0,
    ///     frequencyHz: 440.0,
    ///     amplitude: 0.8,
    ///     phaseRadians: 0.0
    /// );
    /// </code>
    /// </example>
    public static double[] Generate(
        double samplingRate,
        double durationSeconds,
        double frequencyHz,
        double amplitude,
        double phaseRadians)
    {
        if (samplingRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(samplingRate), "Частота дискретизации должна быть больше нуля.");
        if (durationSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Длительность сигнала должна быть больше нуля.");
        if (amplitude < 0.0 || amplitude > 1.0)
            throw new ArgumentOutOfRangeException(nameof(amplitude), "Амплитуда должна находиться в диапазоне от 0.0 до 1.0.");

        int totalSamples = (int)(durationSeconds * samplingRate);
        double[] signal = new double[totalSamples];

        double phaseOffset = phaseRadians / (2.0 * Math.PI);

        for (int n = 0; n < totalSamples; n++)
        {
            double theta = (frequencyHz * n / samplingRate + phaseOffset) % 1.0;

            if (theta < 0)
            {
                theta += 1.0;
            }

            signal[n] = amplitude * (2.0 * theta - 1.0);
        }

        return signal;
    }
}