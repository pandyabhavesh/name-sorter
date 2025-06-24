using name_sorter.application.Data;

namespace name_sorter.application.Interface
{
    public interface IPersonParser
    {
        string FormatLine(Person person);
        Person ParseLine(string line);
    }
}