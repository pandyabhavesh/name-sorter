using System.Globalization;

namespace name_sorter_application.Data;

public readonly record struct Person(
    string FirstName,
    string? MiddleNames, // Nullable to handle cases with no middle name
    string LastName
);


