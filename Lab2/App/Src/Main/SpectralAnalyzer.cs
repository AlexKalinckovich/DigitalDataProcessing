namespace Lab2.App.Src.Main;

public static class SpectralAnalyzer
{
    public static SpectrumResult Analyze(double[] signal, double samplingRate, bool useFft)
    {
        double[] input = signal;
        if (useFft)
        {
            input = PadToPowerOfTwo(signal);
        }

        ComplexNumber[] spectrum = useFft
            ? FastFourierTransform.Forward(input)
            : DiscreteFourierTransform.Forward(input);

        int n = input.Length;
        var frequencies = new double[n];
        var amplitudes = new double[n];
        var phases = new double[n];

        for (int k = 0; k < n; k++)
        {
            frequencies[k] = k * samplingRate / n;
            amplitudes[k] = spectrum[k].Magnitude / n;
            phases[k] = spectrum[k].Phase;
        }

        return new SpectrumResult(frequencies, amplitudes, phases, spectrum);
    }

    public static double[] Reconstruct(SpectrumResult spectrum, bool useFft)
    {
        return useFft
            ? FastFourierTransform.Inverse(spectrum.Spectrum)
            : DiscreteFourierTransform.Inverse(spectrum.Spectrum);
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

public record SpectrumResult(double[] Frequencies, double[] Amplitudes, double[] Phases, ComplexNumber[] Spectrum);