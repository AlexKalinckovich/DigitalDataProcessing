using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;




public static class TriangleSignal
{
    
    public static double[] Generate(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Frequency frequency,
        Amplitude amplitude,
        double phaseRadians = 0.0)
    {
        int samplingRateValue = samplingRate.Value;
        int durationValue = durationInSeconds.Value;
        double frequencyValue = frequency.Value;
        double amplitudeValue = amplitude.Value;

        int totalSamples = durationValue * samplingRateValue;
        double[] signal = new double[totalSamples];

        double phaseOffset = phaseRadians / (2.0 * Math.PI);

        for (int n = 0; n < totalSamples; n++)
        {
            double theta = (frequencyValue * n / samplingRateValue + phaseOffset) % 1.0;
            theta = theta < 0 ? theta + 1.0 : theta;
            signal[n] = amplitudeValue * (1.0 - 4.0 * Math.Abs(theta - 0.5));
        }

        return signal;
    }
}