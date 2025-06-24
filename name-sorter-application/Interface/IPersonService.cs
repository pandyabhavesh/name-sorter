using name_sorter.application.Data;

namespace name_sorter.application.Interface
{
    public interface IPersonService
    {
        List<Person> Load(string path);
        Task<List<Person>> LoadAsync(string path);
        void Save(string path, IEnumerable<Person> people);
        Task SaveAsync(string path, IEnumerable<Person> people);
    }
}