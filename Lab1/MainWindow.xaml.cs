using System.Windows;
using Lab1.App.Src.Main.AudioGeneration;
using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        SubscribeSliderChanges();
        UpdateAllLabels();
    }

    private void SubscribeSliderChanges()
    {
        SamplingRateSlider.ValueChanged += (_, _) => UpdateLabel(SamplingRateLabel, SamplingRateSlider.Value, "0");
        DurationSlider.ValueChanged += (_, _) => UpdateLabel(DurationLabel, DurationSlider.Value, "0");
        AmplitudeSlider.ValueChanged += (_, _) => UpdateLabel(AmplitudeLabel, AmplitudeSlider.Value, "0.###");
        DutyRatioSlider.ValueChanged += (_, _) => UpdateLabel(DutyRatioLabel, DutyRatioSlider.Value, "0.###");
        PhaseSlider.ValueChanged += (_, _) => UpdateLabel(PhaseLabel, PhaseSlider.Value, "0.###");
        FrequencySlider.ValueChanged += (_, _) => UpdateLabel(FrequencyLabel, FrequencySlider.Value, "0");
    }

    private static void UpdateLabel(System.Windows.Controls.TextBlock label, double value, string format)
    {
        label.Text = value.ToString(format);
    }

    private void UpdateAllLabels()
    {
        UpdateLabel(SamplingRateLabel, SamplingRateSlider.Value, "0");
        UpdateLabel(DurationLabel, DurationSlider.Value, "0");
        UpdateLabel(AmplitudeLabel, AmplitudeSlider.Value, "0.###");
        UpdateLabel(DutyRatioLabel, DutyRatioSlider.Value, "0.###");
        UpdateLabel(PhaseLabel, PhaseSlider.Value, "0.###");
        UpdateLabel(FrequencyLabel, FrequencySlider.Value, "0");
    }

    private int SamplingRateValue => (int)SamplingRateSlider.Value;
    private int DurationValue => (int)DurationSlider.Value;
    private double AmplitudeValue => AmplitudeSlider.Value;
    private double DutyRatioValue => DutyRatioSlider.Value;
    private double PhaseValue => PhaseSlider.Value;
    private double FrequencyValue => FrequencySlider.Value;

    private SamplingRate SamplingRateModel => new(SamplingRateValue);
    private DurationInSeconds DurationModel => new(DurationValue);
    private Amplitude AmplitudeModel => new(AmplitudeValue);
    private Frequency FrequencyModel => new(FrequencyValue);

    private void SinusoidButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = Sinusoid.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            phaseRadians: PhaseValue);
        ShowResult(signal);
    }

    private void SawtoothButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = SawtoothSignal.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            phaseRadians: PhaseValue);
        ShowResult(signal);
    }

    private void TriangleButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = TriangleSignal.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            phaseRadians: PhaseValue);
        ShowResult(signal);
    }

    private void PulseButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = PulseSignal.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            dutyRatio: new DutyRatio(DutyRatioValue),
            phaseRadians: PhaseValue);
        ShowResult(signal);
    }

    private void UniformNoiseButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = NoiseSignal.GenerateUniformNoise(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            amplitude: AmplitudeModel);
        ShowResult(signal);
    }

    private void GaussianNoiseButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = NoiseSignal.GenerateGaussianNoise(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            amplitude: AmplitudeModel);
        ShowResult(signal);
    }

    private void PolyphonyButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = Polyphony.Sum(new double[][]
        {
            Sinusoid.Generate(
                samplingRate: SamplingRateModel,
                durationInSeconds: DurationModel,
                frequency: new Frequency(440),
                amplitude: new Amplitude(0.5)),
            Sinusoid.Generate(
                samplingRate: SamplingRateModel,
                durationInSeconds: DurationModel,
                frequency: new Frequency(660),
                amplitude: new Amplitude(0.3))
        });
        ShowResult(signal);
    }

    private void AmModButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = Modulation.AmplitudeModulate(
            carrier: Sinusoid.Generate(
                samplingRate: SamplingRateModel,
                durationInSeconds: DurationModel,
                frequency: FrequencyModel,
                amplitude: AmplitudeModel),
            modulator: Sinusoid.Generate(
                samplingRate: SamplingRateModel,
                durationInSeconds: DurationModel,
                frequency: new Frequency(5),
                amplitude: new Amplitude(1.0)),
            depth: 0.5);
        ShowResult(signal);
    }

    private void FmModButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = Modulation.FrequencyModulate(
            samplingRate: SamplingRateModel,
            amplitude: AmplitudeModel,
            carrierFrequency: FrequencyModel,
            frequencyDeviation: 100.0,
            modulator: SawtoothSignal.Generate(
                samplingRate: SamplingRateModel,
                durationInSeconds: DurationModel,
                frequency: new Frequency(5),
                amplitude: new Amplitude(1.0)));
        ShowResult(signal);
    }

    private void ShowResult(double[] signal)
    {
        var min = signal.Min();
        var max = signal.Max();
        var mean = signal.Average();
        ResultBox.Text = $"Отсчётов: {signal.Length}\nМинимум: {min:F4}\nМаксимум: {max:F4}\nСреднее: {mean:F4}";
    }
}