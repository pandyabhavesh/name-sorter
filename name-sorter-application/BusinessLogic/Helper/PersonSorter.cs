using name_sorter_application.Data;
using name_sorter_application.Interface;

namespace name_sorter_application.BusinessLogic.Helper;

public class PersonSorter : IPersonSorter
{
    /// <summary>
    /// Sorts a List<Person> *in‑place* with minimal allocations.
    /// </summary>
    public void SortByName(List<Person> people)
    {
        ArgumentNullException.ThrowIfNull(people);
   
        people.Sort(PersonNameComparer.Instance);
    }

    public void SortByNameUsingOrderBy(List<Person> people)
    {
        ArgumentNullException.ThrowIfNull(people);

        people.AsParallel()
            .OrderBy(p => p.LastName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.FirstName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.MiddleNames ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .ToList(); // Force evaluation and materialization into a new list
    }
}
