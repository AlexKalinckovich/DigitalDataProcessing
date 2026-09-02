using Lab1.App.Src.Main.AudioGeneration;
using Lab1.App.Src.Main.AudioGeneration.Models;

BuildMainMenu().Show();

Menu BuildMainMenu()
{
    return new Menu(
        title: "Главное меню",
        exitLabel: "Выход",
        options: new[]
        {
            new MenuOption("Сгенерировать сигнал", () => BuildSignalMenu().Show())
        });
}

Menu BuildSignalMenu()
{
    return new Menu(
        title: "Варианты сигналов",
        exitLabel: "Назад",
        options: new[]
        {
            new MenuOption("Синусоида", () => GenerateAndIgnore(
                Sinusoid.Generate(
                    samplingRate: new SamplingRate(44100),
                    durationInSeconds: new DurationInSeconds(2),
                    frequency: new Frequency(440),
                    amplitude: new Amplitude(0.8)))),
            new MenuOption("Пилообразный", () => GenerateAndIgnore(
                SawtoothSignal.Generate(
                    samplingRate: new SamplingRate(44100),
                    durationInSeconds: new DurationInSeconds(2),
                    frequency: new Frequency(440),
                    amplitude: new Amplitude(0.8)))),
            new MenuOption("Треугольный", () => GenerateAndIgnore(
                TriangleSignal.Generate(
                    samplingRate: new SamplingRate(44100),
                    durationInSeconds: new DurationInSeconds(2),
                    frequency: new Frequency(440),
                    amplitude: new Amplitude(0.8)))),
            new MenuOption("Прямоугольный", () => GenerateAndIgnore(
                PulseSignal.Generate(
                    samplingRate: new SamplingRate(44100),
                    durationInSeconds: new DurationInSeconds(2),
                    frequency: new Frequency(440),
                    amplitude: new Amplitude(0.8),
                    phaseRadians: 0.0,
                    dutyRatio: 2.0))),
            new MenuOption("Шум (равномерный)", () => GenerateAndIgnore(
                NoiseSignal.GenerateUniformNoise(
                    samplingRate: new SamplingRate(44100),
                    durationInSeconds: new DurationInSeconds(2),
                    amplitude: new Amplitude(0.8)))),
            new MenuOption("Шум (гауссовский)", () => GenerateAndIgnore(
                NoiseSignal.GenerateGaussianNoise(
                    samplingRate: new SamplingRate(44100),
                    durationInSeconds: new DurationInSeconds(2),
                    amplitude: new Amplitude(0.8))))
        });
}

void GenerateAndIgnore(double[] signal)
{
    Console.WriteLine($"Сгенерировано отсчётов: {signal.Length}. Результат пока игнорируется.");
}

sealed class MenuOption
{
    public string Label { get; }
    public Action Action { get; }

    public MenuOption(string label, Action action)
    {
        Label = label;
        Action = action;
    }
}

sealed class Menu
{
    private readonly string _title;
    private readonly string _exitLabel;
    private readonly IReadOnlyList<MenuOption> _options;

    public Menu(string title, string exitLabel, IReadOnlyList<MenuOption> options)
    {
        _title = title;
        _exitLabel = exitLabel;
        _options = options;
    }

    public void Show()
    {
        int choice;
        while ((choice = ReadChoice()) != -1)
        {
            if (choice >= 1 && choice <= _options.Count)
            {
                _options[choice - 1].Action();
            }
            else
            {
                Console.WriteLine("Пункт не найден.");
            }
        }
    }

    private int ReadChoice()
    {
        Console.WriteLine($"\n=== {_title} ===");
        for (int i = 0; i < _options.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {_options[i].Label}");
        }
        Console.WriteLine($"  -1. {_exitLabel}");
        Console.Write("Выберите пункт: ");

        var input = Console.ReadLine();
        if (!int.TryParse(input, out var choice))
        {
            Console.WriteLine("Некорректный ввод.");
            return ReadChoice();
        }

        return choice;
    }
}