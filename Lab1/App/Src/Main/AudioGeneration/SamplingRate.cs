namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Частота дискретизации — количество отсчётов (чисел), обрабатываемых
/// компьютером за 1 секунду звучания.
/// </summary>
public readonly struct SamplingRate
{
    public int Value { get; }

    public SamplingRate(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Частота дискретизации должна быть больше нуля.");
        Value = value;
    }
}