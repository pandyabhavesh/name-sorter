// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using name_sorter_application.BusinessLogic.Helper;
using name_sorter_application.BusinessLogic.Services;
using name_sorter_application.Interface;
using Serilog;

public static class Program
{
    // Exposed so benchmarks can reach the provider
    public static IServiceProvider Services { get; private set; } = default!;

    public static void Main(string[] args)
    {

        GetServices();

        BenchmarkSwitcher
                   .FromAssembly(typeof(Program).Assembly)
                   .Run(args);

        //BenchmarkRunner.Run<PersonSortBenchmarks>();
        //BenchmarkRunner.Run<LoadPeopleBenchmark>();

        
    }

    public static IServiceProvider GetServices()
    {
        if (Services != null)
        {
            return Services; // already built
        }

        // Build your production-ish service collection
        var services = new ServiceCollection()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<IPersonParser, PersonParser>()
            .AddSingleton<IPersonService, PersonService>()
            .AddSingleton<IPersonSorter, PersonSorter>()
            // … any other business services or configs
            .AddLogging(builder => builder.AddSerilog())  // optional
            .BuildServiceProvider();

        Services = services;        // stash for benchmarks
        return Services; // return the built provider
    }
}