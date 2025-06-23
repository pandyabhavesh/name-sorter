using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using name_sorter;
using name_sorter_application.Interface;
using name_sorter_application.BusinessLogic.Services;
using name_sorter_application.BusinessLogic.Helper;
using Serilog;
using name_sorter_application;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()   // global threshold
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

using IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((context, services) =>
    {
        services.AddNameSorterApplicationServices();
        services.AddSingleton<App>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception ex)
{
    Log.Error(ex, $"An error occurred while running the application. Error: {ex.Message}");
    return;
}
finally
{
    Log.CloseAndFlush();
}


// BenchmarkRunner.Run<PersonSortBenchmarks>();

//var roster = new List<Person>
//{
//    new("Grace",   "H.", null, "Hopper"),
//    new("Alan",  null,  null, "Turing"),
//    new("Barbara", "Bob", "J.", "Liskov"),
//    new("Ada",   null,  null, "Lovelace")
//};




