namespace Lab1.App.Src.Main.AudioGeneration.Models;

public readonly struct DutyRatio
{
    public double Value { get; }
    public DutyRatio(double dutyRatio)
    {
        if (dutyRatio < 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(dutyRatio), "Скважность не может быть меньше 1.0.");
        }
        Value = dutyRatio;
    }
}