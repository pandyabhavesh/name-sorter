using name_sorter.application.Data;

namespace name_sorter.application.Interface
{
    public interface IPersonSorter
    {
        void SortByNameUsingOrderBy(List<Person> people);
        void SortByName(List<Person> people);
    }
}