using Microsoft.Extensions.Logging;
using name_sorter.application.Data;
using name_sorter.application.Interface;
using Serilog.Events;

namespace name_sorter;

public class App(IPersonService personLoader, IPersonSorter personSorter, ILogger<App> log)
{
    const string defaultInputFilePath = @".\unsorted-names-list.txt";
    const string defaultOutputFilePath = @"sorted-names-list.txt";

    /// <summary>
    /// Executes the main workflow of loading, sorting, and saving a list of people.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Loads a list of people
    /// from the specified input file.</item> <item>Sorts the list of people by name using a predefined sorting
    /// method.</item> <item>Saves the sorted list to the specified output file.</item> </list> If logging is enabled,
    /// the method logs the loaded and sorted data, as well as any errors encountered.</remarks>
    /// <param name="args">An array of command-line arguments. The first element specifies the input file path,  and the second element
    /// specifies the output file path. If not provided, default file paths are used. 
    /// default input path unsorted-names-list.txt
    /// default output path sorted-names-list.txt</param>
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
