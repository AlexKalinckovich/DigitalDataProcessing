namespace Lab2.App.Src.Main;

public static class FastFourierTransform
{
    public static ComplexNumber[] Forward(double[] signal)
    {
        int n = signal.Length;
        if (n <= 1)
        {
            return new[] { new ComplexNumber(signal[0], 0.0) };
        }

        int log2 = 0;
        while ((1 << log2) < n)
        {
            log2++;
        }
        if ((1 << log2) != n)
        {
            throw new ArgumentException("Размер сигнала должен быть степенью двойки для БПФ.");
        }

        var buffer = new ComplexNumber[n];
        for (int i = 0; i < n; i++)
        {
            buffer[i] = new ComplexNumber(signal[i], 0.0);
        }

        FftRecursive(buffer, false);

        return buffer;
    }

    public static double[] Inverse(ComplexNumber[] spectrum)
    {
        int n = spectrum.Length;
        var buffer = new ComplexNumber[n];
        for (int i = 0; i < n; i++)
        {
            buffer[i] = spectrum[i];
        }

        FftRecursive(buffer, true);

        var signal = new double[n];
        for (int i = 0; i < n; i++)
        {
            signal[i] = buffer[i].Real / n;
        }

        return signal;
    }

    private static void FftRecursive(ComplexNumber[] a, bool inverse)
    {
        int n = a.Length;
        if (n <= 1)
        {
            return;
        }

        var even = new ComplexNumber[n / 2];
        var odd = new ComplexNumber[n / 2];
        for (int i = 0; i < n / 2; i++)
        {
            even[i] = a[2 * i];
            odd[i] = a[2 * i + 1];
        }

        FftRecursive(even, inverse);
        FftRecursive(odd, inverse);

        double sign = inverse ? 1.0 : -1.0;
        for (int k = 0; k < n / 2; k++)
        {
            double angle = sign * 2.0 * Math.PI * k / n;
            var twiddle = new ComplexNumber(Math.Cos(angle), Math.Sin(angle));
            var t = twiddle * odd[k];
            a[k] = even[k] + t;
            a[k + n / 2] = even[k] - t;
        }
    }
}