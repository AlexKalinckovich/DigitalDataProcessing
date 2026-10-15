using System;
using System.Collections.Generic;
using System.Linq;
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
        ApplyModParamRange();
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

        CarrierFreqSlider.ValueChanged += (_, _) => UpdateLabel(CarrierFreqLabel, CarrierFreqSlider.Value, "0.###");
        CarrierAmpSlider.ValueChanged += (_, _) => UpdateLabel(CarrierAmpLabel, CarrierAmpSlider.Value, "0.###");
        CarrierPhaseSlider.ValueChanged += (_, _) => UpdateLabel(CarrierPhaseLabel, CarrierPhaseSlider.Value, "0.###");
        CarrierDutySlider.ValueChanged += (_, _) => UpdateLabel(CarrierDutyLabel, CarrierDutySlider.Value, "0.###");

        ModulatorFreqSlider.ValueChanged += (_, _) => UpdateLabel(ModulatorFreqLabel, ModulatorFreqSlider.Value, "0.###");
        ModulatorAmpSlider.ValueChanged += (_, _) => UpdateLabel(ModulatorAmpLabel, ModulatorAmpSlider.Value, "0.###");
        ModulatorPhaseSlider.ValueChanged += (_, _) => UpdateLabel(ModulatorPhaseLabel, ModulatorPhaseSlider.Value, "0.###");
        ModulatorDutySlider.ValueChanged += (_, _) => UpdateLabel(ModulatorDutyLabel, ModulatorDutySlider.Value, "0.###");

        ModParamSlider.ValueChanged += (_, _) => UpdateLabel(ModParamLabel, ModParamSlider.Value, "0.###");

        
        
        ModulationTypeCombo.SelectionChanged += (_, _) => ApplyModParamRange();
    }

    private void ApplyModParamRange()
    {
        if (ModParamSlider == null || ModulationTypeCombo == null)
        {
            return;
        }

        ModParamSlider.Minimum = 0;

        if (ModulationTypeCombo.SelectedIndex == 0) 
        {
            ModParamSlider.Maximum = 1;
            ModParamSlider.Value = 0.5;
        }
        else 
        {
            ModParamSlider.Maximum = 1000;
            ModParamSlider.Value = 200;
        }
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

        UpdateLabel(CarrierFreqLabel, CarrierFreqSlider.Value, "0.###");
        UpdateLabel(CarrierAmpLabel, CarrierAmpSlider.Value, "0.###");
        UpdateLabel(CarrierPhaseLabel, CarrierPhaseSlider.Value, "0.###");
        UpdateLabel(CarrierDutyLabel, CarrierDutySlider.Value, "0.###");

        UpdateLabel(ModulatorFreqLabel, ModulatorFreqSlider.Value, "0.###");
        UpdateLabel(ModulatorAmpLabel, ModulatorAmpSlider.Value, "0.###");
        UpdateLabel(ModulatorPhaseLabel, ModulatorPhaseSlider.Value, "0.###");
        UpdateLabel(ModulatorDutyLabel, ModulatorDutySlider.Value, "0.###");

        UpdateLabel(ModParamLabel, ModParamSlider.Value, "0.###");
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

    private double[] GenerateSelectedSignal(int typeIndex, double freq, double amp, double phase, double dutyRatio)
    {
        return typeIndex switch
        {
            0 => Sinusoid.Generate(SamplingRateModel, DurationModel, new Frequency(freq), new Amplitude(amp), phase),
            1 => SawtoothSignal.Generate(SamplingRateModel, DurationModel, new Frequency(freq), new Amplitude(amp), phase),
            2 => TriangleSignal.Generate(SamplingRateModel, DurationModel, new Frequency(freq), new Amplitude(amp), phase),
            3 => PulseSignal.Generate(SamplingRateModel, DurationModel, new Frequency(freq), new Amplitude(amp), new DutyRatio(dutyRatio), phase),
            4 => NoiseSignal.GenerateGaussianNoise(SamplingRateModel, DurationModel, AmplitudeModel),
            _ => NoiseSignal.GenerateGaussianNoise(SamplingRateModel, DurationModel, AmplitudeModel),
        };
    }

    private void GenerateModulatedButton_Click(object sender, RoutedEventArgs e)
    {
        var modulator = GenerateSelectedSignal(
            ModulatorTypeCombo.SelectedIndex,
            ModulatorFreqSlider.Value,
            ModulatorAmpSlider.Value,
            ModulatorPhaseSlider.Value,
            ModulatorDutySlider.Value);

        bool isAm = ModulationTypeCombo.SelectedIndex == 0;
        double[] signal;

        if (isAm)
        {
            var carrier = GenerateSelectedSignal(
                CarrierTypeCombo.SelectedIndex,
                CarrierFreqSlider.Value,
                CarrierAmpSlider.Value,
                CarrierPhaseSlider.Value,
                CarrierDutySlider.Value);

            signal = AmplitudeModulate(carrier, modulator, ModParamSlider.Value);
            SaveSignal(signal, "custom_am_modulated.wav");
        }
        else
        {
            signal = FrequencyModulate(
                samplingRate: SamplingRateModel,
                amplitude: new Amplitude(CarrierAmpSlider.Value),
                carrierFrequency: new Frequency(CarrierFreqSlider.Value),
                frequencyDeviation: ModParamSlider.Value,
                modulator: modulator,
                carrierTypeIndex: CarrierTypeCombo.SelectedIndex,
                carrierDutyRatio: CarrierDutySlider.Value,
                initialPhase: CarrierPhaseSlider.Value,
                out double fMin,
                out double fMax);
            SaveSignal(signal, "custom_fm_modulated.wav", $"\nf_inst: {fMin:F1} … {fMax:F1} Гц");
        }
    }

    private void SaveSignal(double[] signal, string fileName, string extraInfo = "")
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = fileName,
            Filter = "WAV files (*.wav)|*.wav"
        };

        if (dialog.ShowDialog() == true)
        {
            WavWriter.Write(dialog.FileName, SamplingRateModel, signal);
            ShowResult(signal, dialog.FileName, extraInfo);
        }
    }

    private void ShowResult(double[] signal, string filePath, string extraInfo = "")
    {
        var min = signal.Min();
        var max = signal.Max();
        var mean = signal.Average();
        ResultBox.Text = $"Сохранено: {filePath}\nОтсчётов: {signal.Length}\nМинимум: {min:F4}\nМаксимум: {max:F4}\nСреднее: {mean:F4}{extraInfo}";
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

    private static double EvaluateWaveform(int typeIndex, double phase, double dutyRatio)
    {
        const double twoPi = 2.0 * Math.PI;
        double normPhase = (phase % twoPi + twoPi) % twoPi;

        
        double dutyCycle = 1.0 / dutyRatio;

        return typeIndex switch
        {
            0 => Math.Sin(normPhase),
            1 => 2.0 * (normPhase / twoPi) - 1.0,
            2 => normPhase < Math.PI
                ? -1.0 + (2.0 * normPhase / Math.PI)
                : 3.0 - (2.0 * normPhase / Math.PI),
            3 => normPhase < twoPi * dutyCycle ? 1.0 : -1.0,
            _ => Math.Sin(normPhase)
        };
    }

    private static double[] FrequencyModulate(
        SamplingRate samplingRate,
        Amplitude amplitude,
        Frequency carrierFrequency,
        double frequencyDeviation,
        double[] modulator,
        int carrierTypeIndex,
        double carrierDutyRatio,
        double initialPhase,
        out double minInstFrequency,
        out double maxInstFrequency)
    {
        const double twoPi = 2.0 * Math.PI;

        double fs = samplingRate.Value;
        double amp = amplitude.Value;
        double fc = carrierFrequency.Value;
        double nyquist = fs / 2.0;

        double[] result = new double[modulator.Length];
        double phase = initialPhase;

        minInstFrequency = double.MaxValue;
        maxInstFrequency = double.MinValue;

        for (int n = 0; n < modulator.Length; n++)
        {
            
            result[n] = amp * EvaluateWaveform(carrierTypeIndex, phase, carrierDutyRatio);

            
            double fInst = fc + frequencyDeviation * modulator[n];
            fInst = Math.Clamp(fInst, 0.0, nyquist);

            minInstFrequency = Math.Min(minInstFrequency, fInst);
            maxInstFrequency = Math.Max(maxInstFrequency, fInst);

            
            phase += twoPi * fInst / fs;

            
            phase %= twoPi;
        }

        return result;
    }
}