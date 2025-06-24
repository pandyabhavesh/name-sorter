using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using name_sorter.application.BusinessLogic.Helper;
using name_sorter.application.BusinessLogic.Services;
using name_sorter.application.Data;
using name_sorter.application.Interface;
using name_sorter.banchmarks;
using Serilog;


[MemoryDiagnoser]               // Reports allocations alongside timings
[RankColumn]                    // Adds a "Rank" column for quick comparison
[WarmupCount(3)]               // Ensures the code is JIT‑warmed before measurement
[IterationTime(250)]           // 250 ms per iteration reduces noise for I/O heavy work
public class LoadPeopleBenchmark : BenchmarkWithDi
{
    private readonly Random _rand = new(42);
    private string _filePath = string.Empty;
    private IPersonService _peopleService = default!;

    // BenchmarkDotNet automatically runs the benchmark method once per value in Params.
    // Adjust these counts to match realistic workloads for your application.
    [Params(1_000, 10_000, 100_000)]
    public int RecordCount;

    /// <summary>
    /// Creates (or recreates) a temporary file with <see cref="RecordCount"/> random names
    /// and instantiates <see cref="PersonService"/>.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        // (Re)create the temp file so each RecordCount value gets fresh input.
        if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }

        _filePath = Path.Combine(Path.GetTempPath(), $"people_{RecordCount}.txt");

        using (var writer = new StreamWriter(_filePath, false))
        {
            for (int i = 0; i < RecordCount; i++)
            {
                writer.WriteLine(GenerateRandomName());
            }
        }

        _peopleService = Services.GetRequiredService<IPersonService>();
    }

    /// <summary>
    /// Deletes the temporary data file after all iterations complete.
    /// </summary>
    [GlobalCleanup]
    public override void Cleanup()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }

        base.Cleanup();
    }

    /// <summary>
    /// Benchmark target: loads <see cref="RecordCount"/> _people using <see cref="PersonService"/>.
    /// </summary>
    [Benchmark]
    public List<Person> LoadPeople()
    {
        if (_peopleService is null)
            throw new InvalidOperationException("Benchmark not initialised – _loader is null.");

        return _peopleService.Load(_filePath);
    }

    /// <summary>
    /// Loads _people asynchronously using <see cref="PersonService"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [Benchmark]
    public async Task<List<Person>> LoadPeopleAync()
    {
        if (_peopleService is null)
            throw new InvalidOperationException("Benchmark not initialised – _loader is null.");

        return await _peopleService.LoadAsync(_filePath);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Helper utilities
    // ──────────────────────────────────────────────────────────────────────────
    private string GenerateRandomName()
    {
        var firstNames = new[] {
                "Olivia", "Liam", "Charlotte", "Noah", "Amelia", "Oliver",
                "Isla", "Leo", "Ava", "Lucas", "Mia", "Ethan", "Sam", "Bob", "Xi" };

        var middleNames = new[]
        {
                "Grace", "James", "Rose", "Alexander", "Jane", "Michael",
                "Louise", "Matthew", "Claire", "Thomas", "May", "John M", "Tom Johnson" };

        var lastNames = new[] {
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia",
                "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Patel", "Chen", "Zhao" };

        string first = firstNames[_rand.Next(firstNames.Length)];
        string last = lastNames[_rand.Next(lastNames.Length)];

        // Roughly 50 % of records get a middle name
        if (_rand.NextDouble() < 0.5)
        {
            string middle = middleNames[_rand.Next(middleNames.Length)];
            return $"{first} {middle} {last}";
        }

        return $"{first} {last}";
    }
}
