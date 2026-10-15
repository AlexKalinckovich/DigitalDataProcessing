using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Lab2.App.Src.Main;
using Lab2.App.Src.Main.AudioGeneration;
using Lab2.App.Src.Main.AudioGeneration.Models;

namespace Lab2;

public partial class MainWindow : Window
{
    private double[] _signal;
    private SpectrumResult _currentSpectrum;

    public MainWindow()
    {
        InitializeComponent();
        SubscribeSliderChanges();
        UpdateAllLabels();
    }

    private void SubscribeSliderChanges()
    {
        FrequencySlider.ValueChanged += (_, _) => UpdateLabel(FrequencyLabel, FrequencySlider.Value, "0");
        AmplitudeSlider.ValueChanged += (_, _) => UpdateLabel(AmplitudeLabel, AmplitudeSlider.Value, "0.###");
        SamplingRateSlider.ValueChanged += (_, _) => UpdateLabel(SamplingRateLabel, SamplingRateSlider.Value, "0");
        DurationSlider.ValueChanged += (_, _) => UpdateLabel(DurationLabel, DurationSlider.Value, "0.###");
        CutoffSlider.ValueChanged += (_, _) => UpdateLabel(CutoffLabel, CutoffSlider.Value, "0");
        Cutoff2Slider.ValueChanged += (_, _) => UpdateLabel(Cutoff2Label, Cutoff2Slider.Value, "0");
    }

    private static void UpdateLabel(TextBlock label, double value, string format)
    {
        label.Text = value.ToString(format);
    }

    private void UpdateAllLabels()
    {
        UpdateLabel(FrequencyLabel, FrequencySlider.Value, "0");
        UpdateLabel(AmplitudeLabel, AmplitudeSlider.Value, "0.###");
        UpdateLabel(SamplingRateLabel, SamplingRateSlider.Value, "0");
        UpdateLabel(DurationLabel, DurationSlider.Value, "0.###");
        UpdateLabel(CutoffLabel, CutoffSlider.Value, "0");
        UpdateLabel(Cutoff2Label, Cutoff2Slider.Value, "0");
    }

    private double FrequencyValue => FrequencySlider.Value;
    private double AmplitudeValue => AmplitudeSlider.Value;
    private double SamplingRateValue => SamplingRateSlider.Value;
    private double DurationValue => DurationSlider.Value;
    private bool UseFft => MethodCombo.SelectedIndex == 1;

    private double[] GenerateSignal()
    {
        int n = (int)(SamplingRateValue * DurationValue);
        var signal = new double[n];
        double phaseStep = 2.0 * Math.PI * FrequencyValue / SamplingRateValue;

        switch (SignalTypeCombo.SelectedIndex)
        {
            case 0:
                for (int i = 0; i < n; i++)
                {
                    signal[i] = AmplitudeValue * Math.Sin(phaseStep * i);
                }
                break;
            case 1:
                for (int i = 0; i < n; i++)
                {
                    signal[i] = AmplitudeValue * Math.Sin(phaseStep * i) + 0.5 * AmplitudeValue * Math.Sin(2.0 * phaseStep * i);
                }
                break;
            case 2:
                for (int i = 0; i < n; i++)
                {
                    double theta = (FrequencyValue * i / SamplingRateValue) % 1.0;
                    signal[i] = theta < 0.5 ? AmplitudeValue : -AmplitudeValue;
                }
                break;
            case 3:
                for (int i = 0; i < n; i++)
                {
                    double theta = (FrequencyValue * i / SamplingRateValue) % 1.0;
                    signal[i] = AmplitudeValue * (2.0 * theta - 1.0);
                }
                break;
        }

        return signal;
    }

    private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _signal = GenerateSignal();
            _currentSpectrum = SpectralAnalyzer.Analyze(_signal, SamplingRateValue, UseFft);
            ShowSpectrumResult();
        }
        catch (Exception ex)
        {
            ResultBox.Text = "Ошибка: " + ex.Message;
        }
    }

    private void ShowSpectrumResult()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Метод: {(UseFft ? "БПФ" : "ДПФ")}");
        sb.AppendLine($"Отсчётов: {_signal.Length}");
        sb.AppendLine();

        int half = _signal.Length / 2;
        sb.AppendLine("k\tЧастота (Гц)\tАмплитуда\tФаза (рад)");
        int count = Math.Min(half, 12);
        for (int k = 0; k < count; k++)
        {
            sb.AppendLine($"{k}\t{_currentSpectrum.Frequencies[k]:F1}\t{_currentSpectrum.Amplitudes[k]:F6}\t{_currentSpectrum.Phases[k]:F4}");
        }

        int maxIndex = ArgMax(_currentSpectrum.Amplitudes, 1);
        sb.AppendLine();
        sb.AppendLine($"Максимальная амплитуда: {_currentSpectrum.Amplitudes[maxIndex]:F6} на частоте {_currentSpectrum.Frequencies[maxIndex]:F1} Гц (k={maxIndex})");

        ResultBox.Text = sb.ToString();
    }

    private void ReconstructButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_currentSpectrum == null)
            {
                _signal = GenerateSignal();
                _currentSpectrum = SpectralAnalyzer.Analyze(_signal, SamplingRateValue, UseFft);
            }

            double[] reconstructed = SpectralAnalyzer.Reconstruct(_currentSpectrum, UseFft);
            double maxError = 0.0;
            for (int i = 0; i < _signal.Length; i++)
            {
                maxError = Math.Max(maxError, Math.Abs(reconstructed[i] - _signal[i]));
            }

            ResultBox.Text = $"Сигнал восстановлен методом {(UseFft ? "БПФ" : "ДПФ")}.\n\n" +
                             $"Максимальная абсолютная погрешность: {maxError:E4}\n" +
                             $"Среднеквадратичная погрешность: {RootMeanSquareError(reconstructed, _signal):E4}";
        }
        catch (Exception ex)
        {
            ResultBox.Text = "Ошибка: " + ex.Message;
        }
    }

    private void ErrorButton_Click(object sender, RoutedEventArgs e)
    {
        AnalyzeButton_Click(sender, e);
        ReconstructButton_Click(sender, e);
    }

    private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            double[] input = GenerateSignal();

            double[] filtered = DigitalFilter.Apply(
                input,
                SamplingRateValue,
                GetFilterType(),
                CutoffSlider.Value,
                Cutoff2Slider.Value);

            _signal = filtered;
            _currentSpectrum = SpectralAnalyzer.Analyze(filtered, SamplingRateValue, UseFft);
            ResultBox.Text = BuildFilterResult(input, filtered);
        }
        catch (Exception ex)
        {
            ResultBox.Text = "Ошибка: " + ex.Message;
        }
    }

    private string BuildFilterResult(double[] input, double[] filtered)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Применён фильтр: {FilterTypeCombo.Text}");
        sb.AppendLine($"Частота среза: {CutoffSlider.Value:F1} Гц");
        if (GetFilterType() == FilterType.BandPass || GetFilterType() == FilterType.BandStop)
        {
            sb.AppendLine($"Вторая частота: {Cutoff2Slider.Value:F1} Гц");
        }
        sb.AppendLine($"Отсчётов после фильтрации: {filtered.Length}");
        sb.AppendLine();

        sb.AppendLine($"Максимум исходного сигнала: {MaxAbs(input):F4}");
        sb.AppendLine($"Максимум отфильтрованного сигнала: {MaxAbs(filtered):F4}");
        return sb.ToString();
    }

    private void SaveFilteredButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = "filtered.wav",
            Filter = "WAV files (*.wav)|*.wav"
        };

        if (dialog.ShowDialog() == true)
        {
            var samplingRate = new SamplingRate((int)SamplingRateValue);
            WavWriter.Write(dialog.FileName, samplingRate, _signal);
            ResultBox.Text += $"\nСохранено: {dialog.FileName}";
        }
    }

    private FilterType GetFilterType() => FilterTypeCombo.SelectedIndex switch
    {
        0 => FilterType.LowPass,
        1 => FilterType.HighPass,
        2 => FilterType.BandPass,
        _ => FilterType.BandStop
    };

    private void FilterTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Cutoff2Text == null || Cutoff2Slider == null || Cutoff2Label == null)
        {
            return;
        }

        bool isBand = FilterTypeCombo.SelectedIndex == 2 || FilterTypeCombo.SelectedIndex == 3;
        var visibility = isBand ? Visibility.Visible : Visibility.Collapsed;
        Cutoff2Text.Visibility = visibility;
        Cutoff2Slider.Visibility = visibility;
        Cutoff2Label.Visibility = visibility;
    }

    private static int ArgMax(double[] values, int startIndex)
    {
        int maxIndex = startIndex;
        for (int i = startIndex + 1; i < values.Length; i++)
        {
            if (values[i] > values[maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private static double RootMeanSquareError(double[] reconstructed, double[] original)
    {
        int n = Math.Min(reconstructed.Length, original.Length);
        double sum = 0.0;
        for (int i = 0; i < n; i++)
        {
            double error = reconstructed[i] - original[i];
            sum += error * error;
        }
        return Math.Sqrt(sum / n);
    }

    private static double MaxAbs(double[] values)
    {
        double max = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            max = Math.Max(max, Math.Abs(values[i]));
        }
        return max;
    }
}