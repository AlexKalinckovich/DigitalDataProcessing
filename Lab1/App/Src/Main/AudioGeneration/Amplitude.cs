namespace Lab1.App.Src.Main.AudioGeneration;

/// <summary>
/// Амплитуда — пиковая высота сигнала, определяющая громкость:
/// 0.0 (тишина) .. 1.0 (максимум без искажений).
/// </summary>
public readonly struct Amplitude
{
    public double Value { get; }

    public Amplitude(double value)
    {
        if (value < 0.0 || value > 1.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Амплитуда должна быть в диапазоне [0.0, 1.0].");
        Value = value;
    }
}