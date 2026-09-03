namespace Lab1.App.Src.Main.AudioGeneration.Models;

/// <summary>
/// Длительность сигнала — общее время звучания в секундах (целое число секунд).
/// </summary>
public readonly struct DurationInSeconds
{
    public int Value { get; }

    public DurationInSeconds(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Длительность должна быть больше нуля.");
        Value = value;
    }
}