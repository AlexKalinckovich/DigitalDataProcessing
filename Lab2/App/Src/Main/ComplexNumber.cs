namespace Lab2.App.Src.Main;

public record ComplexNumber(double Real, double Imaginary)
{
    public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);
    public double Phase => Math.Atan2(Imaginary, Real);

    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b) => new(a.Real + b.Real, a.Imaginary + b.Imaginary);
    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b) => new(a.Real - b.Real, a.Imaginary - b.Imaginary);
    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b) => new(a.Real * b.Real - a.Imaginary * b.Imaginary, a.Real * b.Imaginary + a.Imaginary * b.Real);

    public static ComplexNumber FromPolar(double magnitude, double phase) => new(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
}