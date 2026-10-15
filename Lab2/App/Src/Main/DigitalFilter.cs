namespace Lab2.App.Src.Main;

public enum FilterType
{
    LowPass,
    HighPass,
    BandPass,
    BandStop
}

public static class DigitalFilter
{
    public static double[] Apply(double[] signal, double samplingRate, FilterType type, double cutoff, double cutoff2)
    {
        double[] input = PadToPowerOfTwo(signal);
        int n = input.Length;

        double[] filterSpectrum = BuildFrequencyResponse(n, samplingRate, type, cutoff, cutoff2);
        ComplexNumber[] spectrum = FastFourierTransform.Forward(input);
        var filtered = new ComplexNumber[n];
        for (int i = 0; i < n; i++)
        {
            filtered[i] = new ComplexNumber(spectrum[i].Real * filterSpectrum[i], spectrum[i].Imaginary * filterSpectrum[i]);
        }

        double[] result = FastFourierTransform.Inverse(filtered);

        if (result.Length != signal.Length)
        {
            var trimmed = new double[signal.Length];
            Array.Copy(result, trimmed, signal.Length);
            return trimmed;
        }

        return result;
    }

    private static double[] BuildFrequencyResponse(int n, double samplingRate, FilterType type, double cutoff, double cutoff2)
    {
        double nyquist = samplingRate / 2.0;
        var response = new double[n];

        for (int k = 0; k < n; k++)
        {
            double frequency = k * samplingRate / n;
            if (frequency > nyquist)
            {
                frequency = samplingRate - frequency;
            }

            double gain = type switch
            {
                FilterType.LowPass => frequency <= cutoff ? 1.0 : 0.0,
                FilterType.HighPass => frequency >= cutoff ? 1.0 : 0.0,
                FilterType.BandPass => frequency >= cutoff && frequency <= cutoff2 ? 1.0 : 0.0,
                FilterType.BandStop => frequency < cutoff || frequency > cutoff2 ? 1.0 : 0.0,
                _ => 1.0
            };

            response[k] = gain;
        }

        return response;
    }

    private static double[] PadToPowerOfTwo(double[] signal)
    {
        int n = signal.Length;
        int paddedLength = 1;
        while (paddedLength < n)
        {
            paddedLength <<= 1;
        }

        if (paddedLength == n)
        {
            return signal;
        }

        var padded = new double[paddedLength];
        Array.Copy(signal, padded, n);
        return padded;
    }
}