using name_sorter_application.Data;

namespace name_sorter_application.Interface
{
    public interface IPersonParser
    {
        string FormatLine(Person person);
        Person ParseLine(string line);
    }
}