using name_sorter.application.Data;
using name_sorter.application.Interface;

namespace name_sorter.application.BusinessLogic.Helper;

public class PersonParser : IPersonParser
{
    public string FormatLine(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        var parts = new[] { person.FirstName, person.MiddleNames ?? string.Empty, person.LastName };
        // Handle middle names being null or empty
        return string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }

    /// <summary>
    /// Parses a line of text into a <see cref="Person"/> object.
    /// Splits the line by whitespace, assuming 
    /// the first part is the first name,
    /// the last part is the last name,
    /// and any parts in between are middle names.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public Person ParseLine(string line)
    {
        // Split on whitespace, remove empties, preserve internal casing
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Ensure we have at least a first and last name
        if (parts.Length is < 2)
            throw new FormatException($"Invalid person name format: \"{line}\". Expected at least a first and last name, but got {parts.Length} parts.");

        string first = parts.First(); 
        string last = parts.Last();
        string? middleNames = parts.Length > 2 ? string.Join(" ", parts.Skip(1).SkipLast(1)) : null;

        return new Person(first, middleNames, last);
    }
}
