using Lab1.App.Src.Main.AudioGeneration.Models;

namespace Lab1.App.Src.Main.AudioGeneration;




public static class PulseSignal
{
    
    
    
    
    
    public static double[] Generate(
        SamplingRate samplingRate,
        DurationInSeconds durationInSeconds,
        Frequency frequency,
        Amplitude amplitude,
        DutyRatio dutyRatio,
        double phaseRadians)
    {
        int samplingRateValue = samplingRate.Value;
        int durationValue = durationInSeconds.Value;
        double frequencyValue = frequency.Value;
        double amplitudeValue = amplitude.Value;
        double dutyRatioValue = dutyRatio.Value;
        
        int totalSamples = durationValue * samplingRateValue;
        double[] signal = new double[totalSamples];

        double dutyCycle = 1.0 / dutyRatioValue;

        
        double phaseOffset = phaseRadians / (2.0 * Math.PI);

        for (int n = 0; n < totalSamples; n++)
        {
            
            double theta = (frequencyValue * n / samplingRateValue + phaseOffset) % 1.0;
            theta = theta < 0 ? theta + 1.0 : theta;
            signal[n] = (theta < dutyCycle) ? amplitudeValue : -amplitudeValue;
        }
        return signal;
    }
}