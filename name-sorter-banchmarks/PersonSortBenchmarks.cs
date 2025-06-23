using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using name_sorter_application.BusinessLogic.Helper;
using name_sorter_application.BusinessLogic.Services;
using name_sorter_application.Data;
using name_sorter_application.Interface;
using name_sorter_banchmarks;
using Serilog;

[MemoryDiagnoser]
public class PersonSortBenchmarks : BenchmarkWithDi
{
    [Params(1_000, 10_000, 100_000)]
    public int Count;

    private List<Person>? _people;
    private IPersonSorter _personSorter;

    [GlobalSetup]
    public void Setup()
    {
        var rand = new Random(42);
        _people = new List<Person>(Count);
        _personSorter = Services.GetRequiredService<IPersonSorter>(); ;

        for (int i = 0; i < Count; i++)
        {
            _people.Add(new Person(
                FirstName: RandomString(rand),
                MiddleNames: rand.Next(0, 2) == 0 ? null : RandomString(rand),
                LastName: RandomString(rand)
            ));
        }
    }

    private static string RandomString(Random rand)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int len = rand.Next(3, 8);
        return new string(Enumerable.Range(0, len).Select(_ => chars[rand.Next(chars.Length)]).ToArray());
    }

    [Benchmark(Baseline = true)]
    public void CustomSorter()
    {
        var copyOfPeople = new List<Person>(_people);
        _personSorter.SortByName(copyOfPeople);
    }

    [Benchmark]
    public void PLINQ_OrderBy()
    {
        var sorted = _people
            .OrderBy(p => p.LastName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.FirstName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.MiddleNames ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    [Benchmark]
    public void PLINQ_OrderBy_AsParallel()
    {
        var sorted = _people
            .AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount)
            .OrderBy(p => p.LastName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.FirstName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.MiddleNames ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
