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
        SaveSignal(Sinusoid.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            phaseRadians: PhaseValue), "sinusoid.wav");
    }

    private void SawtoothButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSignal(SawtoothSignal.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            phaseRadians: PhaseValue), "sawtooth.wav");
    }

    private void TriangleButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSignal(TriangleSignal.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            phaseRadians: PhaseValue), "triangle.wav");
    }

    private void PulseButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSignal(PulseSignal.Generate(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            frequency: FrequencyModel,
            amplitude: AmplitudeModel,
            dutyRatio: new DutyRatio(DutyRatioValue),
            phaseRadians: PhaseValue), "pulse.wav");
    }

    private void UniformNoiseButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSignal(NoiseSignal.GenerateUniformNoise(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            amplitude: AmplitudeModel), "uniform_noise.wav");
    }

    private void GaussianNoiseButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSignal(NoiseSignal.GenerateGaussianNoise(
            samplingRate: SamplingRateModel,
            durationInSeconds: DurationModel,
            amplitude: AmplitudeModel), "gaussian_noise.wav");
    }

    private void PolyphonyButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = Sum(new double[][]
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
        SaveSignal(signal, "polyphony.wav");
    }

    private void AmModButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = AmplitudeModulate(
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
        SaveSignal(signal, "am_modulated.wav");
    }

    private void FmModButton_Click(object sender, RoutedEventArgs e)
    {
        var signal = FrequencyModulate(
            samplingRate: SamplingRateModel,
            amplitude: AmplitudeModel,
            carrierFrequency: FrequencyModel,
            frequencyDeviation: 100.0,
            modulator: SawtoothSignal.Generate(
                samplingRate: SamplingRateModel,
                durationInSeconds: DurationModel,
                frequency: new Frequency(5),
                amplitude: new Amplitude(1.0)));
        SaveSignal(signal, "fm_modulated.wav");
    }

    private void SaveSignal(double[] signal, string fileName)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = fileName,
            Filter = "WAV files (*.wav)|*.wav"
        };

        if (dialog.ShowDialog() == true)
        {
            WavWriter.Write(dialog.FileName, SamplingRateModel, signal);
            ShowResult(signal, dialog.FileName);
        }
    }

    private void ShowResult(double[] signal, string filePath)
    {
        var min = signal.Min();
        var max = signal.Max();
        var mean = signal.Average();
        ResultBox.Text = $"Сохранено: {filePath}\nОтсчётов: {signal.Length}\nМинимум: {min:F4}\nМаксимум: {max:F4}\nСреднее: {mean:F4}";
    }

    private static double[] Sum(IReadOnlyList<double[]> signals)
    {
        double[] result = new double[signals[0].Length];
        for (int n = 0; n < result.Length; n++)
        {
            double sum = 0.0;
            for (int i = 0; i < signals.Count; i++)
            {
                sum += signals[i][n];
            }
            result[n] = sum;
        }

        return result;
    }

    private static double[] AmplitudeModulate(double[] carrier, double[] modulator, double depth)
    {
        double[] result = new double[carrier.Length];
        for (int n = 0; n < carrier.Length; n++)
        {
            result[n] = carrier[n] * (1.0 + depth * modulator[n]);
        }

        return result;
    }

    private static double[] FrequencyModulate(
        SamplingRate samplingRate,
        Amplitude amplitude,
        Frequency carrierFrequency,
        double frequencyDeviation,
        double[] modulator)
    {
        double samplingRateValue = samplingRate.Value;
        double amplitudeValue = amplitude.Value;
        double carrierValue = carrierFrequency.Value;

        double[] result = new double[modulator.Length];
        double phase = 0.0;
        for (int n = 0; n < modulator.Length; n++)
        {
            phase += 2.0 * Math.PI * (carrierValue + frequencyDeviation * modulator[n]) / samplingRateValue;
            result[n] = amplitudeValue * Math.Sin(phase);
        }

        return result;
    }
}