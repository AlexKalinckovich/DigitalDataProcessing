namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Частота колебаний — количество полных периодов синусоиды за одну секунду;
/// определяет высоту звука (в Гц).
/// </summary>
public readonly struct Frequency
{
    public double Value { get; }

    public Frequency(double value)
    {
        if (value <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Частота должна быть больше нуля.");
        Value = value;
    }
}