namespace Lab2.App.Src.Main;

public static class DiscreteFourierTransform
{
    public static ComplexNumber[] Forward(double[] signal)
    {
        int n = signal.Length;
        var spectrum = new ComplexNumber[n];

        for (int k = 0; k < n; k++)
        {
            double real = 0.0;
            double imag = 0.0;
            for (int t = 0; t < n; t++)
            {
                double angle = -2.0 * Math.PI * k * t / n;
                real += signal[t] * Math.Cos(angle);
                imag += signal[t] * Math.Sin(angle);
            }
            spectrum[k] = new ComplexNumber(real, imag);
        }

        return spectrum;
    }

    public static double[] Inverse(ComplexNumber[] spectrum)
    {
        int n = spectrum.Length;
        var signal = new double[n];

        for (int t = 0; t < n; t++)
        {
            double real = 0.0;
            double imag = 0.0;
            for (int k = 0; k < n; k++)
            {
                double angle = 2.0 * Math.PI * k * t / n;
                real += spectrum[k].Real * Math.Cos(angle) - spectrum[k].Imaginary * Math.Sin(angle);
                imag += spectrum[k].Real * Math.Sin(angle) + spectrum[k].Imaginary * Math.Cos(angle);
            }
            signal[t] = real / n;
        }

        return signal;
    }
}