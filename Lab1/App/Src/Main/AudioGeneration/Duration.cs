namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Длительность сигнала — общее время звучания в секундах (целое число секунд).
/// </summary>
public readonly struct Duration
{
    public int Value { get; }

    public Duration(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Длительность должна быть больше нуля.");
        Value = value;
    }
}