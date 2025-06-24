using name_sorter.application.Data;
using System.Globalization;

namespace name_sorter.application.BusinessLogic.Helper;

/// <summary>
/// Performs an ordinal‑ignore‑case comparison:
///   1. LastName
///   2. FirstName
///   3. MiddleNames (nulls last)
/// </summary>
public sealed class PersonNameComparer : IComparer<Person>
{
    private static readonly CompareInfo _cmp = CultureInfo.InvariantCulture.CompareInfo;
    private const CompareOptions _opts = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;

    public static readonly PersonNameComparer Instance = new();

    private PersonNameComparer() { }

    public int Compare(Person x, Person y)
    {
        // 1️. Last name
        int c = _cmp.Compare(x.LastName, y.LastName, _opts);
        if (c != 0) return c;

        // 2️. First name
        c = _cmp.Compare(x.FirstName, y.FirstName, _opts);
        if (c != 0) return c;

        // 3. Middle names (handle nulls)
        return _cmp.Compare(x.MiddleNames ?? string.Empty,
                            y.MiddleNames ?? string.Empty,
                            _opts);
    }
}
