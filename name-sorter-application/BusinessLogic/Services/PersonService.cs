using Microsoft.Extensions.Logging;
using name_sorter_application.Data;
using name_sorter_application.Interface;

namespace name_sorter_application.BusinessLogic.Services;

public class PersonService(IFileService fileService, IPersonParser parser, ILogger<PersonService> log) : IPersonService
{
    /// <summary>
    /// Loads a list of <see cref="Person"/> objects from the specified file path.
    /// </summary>
    /// <remarks>This method processes the file in parallel to improve performance when handling large files.
    /// Each line is parsed into a <see cref="Person"/> object using the provided parser. Lines that are empty or
    /// consist only of whitespace are ignored.</remarks>
    /// <param name="path">The path to the file containing the data to load. The file must exist and be accessible.</param>
    /// <returns>A list of <see cref="Person"/> objects parsed from the file. Returns an empty list if the file contains no valid
    /// data.</returns>
    public List<Person> Load(string path)
    {
        // File.ReadLines streams the file lazily and keeps memory usage low
        return fileService.ReadLines(path)
                   .AsParallel()          // uses all cores
                   .WithDegreeOfParallelism(Environment.ProcessorCount)
                   .Where(line => !string.IsNullOrWhiteSpace(line))
                   .Select(line => parser.ParseLine(line))
                   .ToList();          // materialise into List<Person>
    }

    /// <summary>
    /// Saves a collection of people to a file at the specified path.
    /// </summary>
    /// <remarks>If the <paramref name="people"/> collection is null or empty, the method will log a debug
    /// message and skip the file write operation. Each person in the collection is formatted into a line using the
    /// parser before being written to the file.</remarks>
    /// <param name="path">The file path where the collection of people will be saved. Must be a valid file path.</param>
    /// <param name="people">The collection of <see cref="Person"/> objects to save. Cannot be null or empty.</param>
    public void Save(string path, IEnumerable<Person> people)
    {
        if (people == null || !people.Any())
        {
            log.LogDebug("No people to save, skipping file write operation.");
            return; // nothing to save
        }
            
        var lines = people.Select(p => parser.FormatLine(p)).ToList();
        fileService.WriteLines(path, lines);
    }

    /// <summary>
    /// Asynchronously loads a list of <see cref="Person"/> objects from a file at the specified path.
    /// </summary>
    /// <remarks>Each non-empty, non-whitespace line in the file is parsed into a <see cref="Person"/> object
    /// using  the provided parser. Lines that are empty or consist only of whitespace are ignored.</remarks>
    /// <param name="path">The file path to read from. Must not be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Person"/>
    /// objects parsed from the file. If the file is empty or contains no valid lines,  the returned list will be empty.</returns>
    public async Task<List<Person>> LoadAsync(string path)
    {
        var people = new List<Person>();

        await foreach (var line in fileService.ReadLinesAsync(path))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                people.Add(parser.ParseLine(line));
            }                
        }

        return people;
    }

    /// <summary>
    /// Saves a collection of people to the specified file path asynchronously.
    /// </summary>
    /// <remarks>If the <paramref name="people"/> collection is null or empty, the method returns
    /// immediately without performing any operation.</remarks>
    /// <param name="path">The file path where the collection of people will be saved. Cannot be null or empty.</param>
    /// <param name="people">The collection of <see cref="Person"/> objects to save. Must not be null or empty.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(string path, IEnumerable<Person> people)
    {
        if (people == null || !people.Any()) 
        { 
            log.LogDebug("No people to save, skipping file write operation.");
            return; // nothing to save
        }

        await fileService.WriteLinesAsync(
            path, 
            people.Select(p => parser.FormatLine(p)));
    }
}
