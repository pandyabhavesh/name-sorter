using name_sorter_application.Data;

namespace name_sorter_application.Interface
{
    public interface IPersonSorter
    {
        void SortByNameUsingOrderBy(List<Person> people);
        void SortByName(List<Person> people);
    }
}