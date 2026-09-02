
namespace Lab1.App.Src.Main.AudioGeneration
{
    /// <summary>
    /// Предоставляет методы для генерации цифровых треугольных сигналов.
    /// Треугольная волна линейно нарастает и спадает, не имея разрывов, 
    /// что делает её спектр богаче синусоиды, но мягче прямоугольного импульса.
    /// </summary>
    public static class TriangleSignal
    {
        /// <summary>
        /// Генерирует массив отсчетов, описывающий симметричный треугольный сигнал.
        /// </summary>
        /// <param name="samplingRate">
        /// Частота дискретизации ($f_d$) — количество отсчетов (чисел) в секунду.
        /// </param>
        /// <param name="durationSeconds">
        /// Длительность сигнала ($t_{\text{dur}}$) — общее время звучания в секундах.
        /// </param>
        /// <param name="frequencyHz">
        /// Частота сигнала ($F$) — количество полных треугольных циклов в секунду (в Гц).
        /// </param>
        /// <param name="amplitude">
        /// Амплитуда ($A$) — пиковая высота вершины треугольника (диапазон от $0.0$ до $1.0$).
        /// </param>
        /// <param name="phaseRadians">
        /// Начальная фаза ($\phi$) — начальный сдвиг сигнала по оси времени (в радианах).
        /// </param>
        /// <returns>
        /// Массив вещественных чисел (<c>double[]</c>), представляющий цифровой треугольный сигнал.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если параметры выходят за физические пределы 
        /// (например, амплитуда вне диапазона $[0.0, 1.0]$ или отрицательная длительность).
        /// </exception>
        /// <example>
        /// <code>
        /// // Генерация 1-секундного треугольного сигнала частотой 440 Гц
        /// double[] triangleData = TriangleSignal.Generate(
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
            // Валидация входных параметров
            if (samplingRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(samplingRate), "Частота дискретизации должна быть больше нуля.");
            if (durationSeconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Длительность сигнала должна быть больше нуля.");
            if (amplitude < 0.0 || amplitude > 1.0)
                throw new ArgumentOutOfRangeException(nameof(amplitude), "Амплитуда должна находиться в диапазоне от 0.0 до 1.0.");

            // Шаг 1. Вычисляем количество точек и создаем массив
            int totalSamples = (int)(durationSeconds * samplingRate);
            double[] signal = new double[totalSamples];

            // Предварительный расчет фазового сдвига (оптимизация: выносим из цикла)
            double phaseOffset = phaseRadians / (2.0 * Math.PI);

            // Шаг 2, 3 и 4. Цикл генерации с расчетом относительной фазы и формулой модуля
            for (int n = 0; n < totalSamples; n++)
            {
                // Находим относительную фазу на интервале [0.0, 1.0)
                double theta = (frequencyHz * n / samplingRate + phaseOffset) % 1.0;

                // Корректируем, если фаза ушла в минус (особенность оператора % в C#)
                if (theta < 0)
                {
                    theta += 1.0;
                }

                // Вычисляем значение по формуле с модулем
                signal[n] = amplitude * (1.0 - 4.0 * Math.Abs(theta - 0.5));
            }

            return signal;
        }
    }
}