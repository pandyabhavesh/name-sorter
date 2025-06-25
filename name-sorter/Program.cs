using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using name_sorter;
using Serilog;
using name_sorter.application;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()   // global threshold
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        using IHost host = Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddNameSorterApplicationServices();
                services.AddTransient<App>();
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
    }
}





