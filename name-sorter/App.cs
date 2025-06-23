using Microsoft.Extensions.Logging;
using name_sorter_application.Data;
using name_sorter_application.Interface;
using Serilog;
using Serilog.Events;

namespace name_sorter;

public class App(IPersonService personLoader, IPersonSorter personSorter, ILogger<App> log)
{
    const string defaultInputFilePath = @".\unsorted-names-list.txt";
    const string defaultOutputFilePath = @"sorted-names-list.txt";

    public void Run(string[] args)
    {
        try
        {
            string inputFilePath = args.Length > 0 ? args[0] : defaultInputFilePath;
            string outputFilePath = args.Length > 1 ? args[1] : defaultOutputFilePath;

            List<Person> people = personLoader.Load(inputFilePath);
            log.LogInformation($"Loaded {people.Count:N0} people from {inputFilePath}.");
            
            personSorter.SortByNameUsingOrderBy(people);

            personLoader.Save(outputFilePath, people);
            log.LogInformation($"Sorted names saved to {outputFilePath}.");

            if (log.IsEnabled((LogLevel)LogEventLevel.Information))
            {
                log.LogInformation("Sorted names list:");
                people.ForEach(p => log.LogInformation($"{p.LastName}, {p.FirstName} {p.MiddleNames}"));
            }
        }
        catch (Exception ex)
        {
            log.LogError($"Error: {ex.Message}");
        }
    }
}
