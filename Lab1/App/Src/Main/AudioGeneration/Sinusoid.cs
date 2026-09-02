namespace Lab1.App.Src.Main.AudioGeneration;

public static class Sinusoid
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="samplingRate"></param>
    /// <param name="durationSeconds"></param>
    /// <param name="frequencyHz">
    /// Частота колебаний - высота звука или количество полных периодов
    /// (вверх-вниз) синусоиды за одну секунду
    /// </param>
    /// <param name="amplitude">
    /// Амплитуда -- максимальная высота волны, определяющая громкость
    /// от 0.0 (тишина) до 1.0 (максимальная громкость без искажений).
    /// </param>
    /// <param name="phaseRadians">
    /// Начальная фаза -- сдвиг волны по времени
    /// Определяет, из какой точки (с нуля, с вершины или с середины) синусоида начнет
    /// свое движение в самый первый момент времени t = 0
    /// </param>
    /// <returns></returns>
    public static double[] Generate(
        int samplingRate,
        int durationSeconds,
        double frequencyHz,
        double amplitude = 1.0,
        double phaseRadians = 0.0)
    {
        int totalSamples = durationSeconds * samplingRate;
        double[] signal = new double[totalSamples];
        
        double angularFrequency = 2.0 * Math.PI * frequencyHz / samplingRate;
        for (int n = 0; n < totalSamples; n++)
        {
            signal[n] = amplitude * Math.Sin(angularFrequency * n + phaseRadians);
        }

        return signal;
    }
}