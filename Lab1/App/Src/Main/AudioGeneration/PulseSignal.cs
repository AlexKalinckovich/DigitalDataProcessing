namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Предоставляет методы для генерации цифровых импульсных сигналов.
/// Биполярный импульс переключается между +A и -A, что исключает вредную 
/// постоянную составляющую в акустических системах.
/// </summary>
public static class PulseSignal
{
    /// <summary>
    /// Генерирует массив отсчетов, описывающий прямоугольный импульсный сигнал с заданной скважностью.
    /// </summary>
    /// <param name="samplingRate">
    /// Частота дискретизации ($f_d$) — количество отсчетов (чисел) в секунду.
    /// </param>
    /// <param name="durationSeconds">
    /// Длительность сигнала ($t_{\text{dur}}$) — общее время звучания в секундах.
    /// </param>
    /// <param name="frequencyHz">
    /// Частота сигнала ($F$) — количество полных импульсных циклов в секунду (в Гц).
    /// </param>
    /// <param name="amplitude">
    /// Амплитуда ($A$) — пиковая высота импульса (динамический диапазон от $0.0$ до $1.0$).
    /// </param>
    /// <param name="phaseRadians">
    /// Начальная фаза ($\phi$) — начальный сдвиг сигнала по оси времени (в радианах).
    /// </param>
    /// <param name="dutyRatio">
    /// Скважность ($S$) — отношение периода повторения к длительности импульса (например, $2$ для меандра).
    /// </param>
    /// <returns>
    /// Массив вещественных чисел (<c>double[]</c>), представляющий цифровой импульсный сигнал.
    /// Значения в массиве строго равны $+A$ или $-A$.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если параметры выходят за физические пределы 
    /// (например, скважность $< 1.0$ или амплитуда вне диапазона $[0.0, 1.0]$).
    /// </exception>
    /// <example>
    /// <code>
    /// // Генерация 1-секундного меандра (скважность 2) частотой 1000 Гц
    /// double[] pulseData = PulseSignal.Generate(
    ///     samplingRate: 44100,
    ///     durationSeconds: 1.0,
    ///     frequencyHz: 1000.0,
    ///     amplitude: 0.8,
    ///     phaseRadians: 0.0,
    ///     dutyRatio: 2.0
    /// );
    /// </code>
    /// </example>
    public static double[] Generate(
        double samplingRate,
        double durationSeconds,
        double frequencyHz,
        double amplitude,
        double phaseRadians,
        double dutyRatio)
    {
        // Валидация входных параметров
        if (samplingRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(samplingRate), "Частота дискретизации должна быть больше нуля.");
        if (durationSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Длительность сигнала должна быть больше нуля.");
        if (amplitude < 0.0 || amplitude > 1.0)
            throw new ArgumentOutOfRangeException(nameof(amplitude), "Амплитуда должна находиться в диапазоне от 0.0 до 1.0.");
        if (dutyRatio < 1.0)
            throw new ArgumentOutOfRangeException(nameof(dutyRatio), "Скважность не может быть меньше 1.0 (длительность импульса не может превышать период).");

        // Шаг 1. Вычисление размера массива отсчетов
        int totalSamples = (int)(durationSeconds * samplingRate);
        double[] signal = new double[totalSamples];

        // Шаг 2. Перевод скважности в коэффициент заполнения (Duty Cycle)
        double dutyCycle = 1.0 / dutyRatio;

        // Шаг 3 и 4. Итерационный расчет каждого отсчета и ветвление по уровню сигнала
        double phaseOffset = phaseRadians / (2.0 * Math.PI);

        for (int n = 0; n < totalSamples; n++)
        {
            // Нормированная фаза на периоде в интервале [0.0, 1.0)
            double theta = (frequencyHz * n / samplingRate + phaseOffset) % 1.0;

            // Коррекция на случай отрицательной начальной фазы (остаток от деления в C# может быть отрицательным)
            if (theta < 0)
            {
                theta += 1.0;
            }

            // Формирование прямоугольного перепада амплитуды
            signal[n] = (theta < dutyCycle) ? amplitude : -amplitude;
        }

        return signal;
    }
}