namespace name_sorter_application.Interface
{
    public interface IFileService
    {
        IEnumerable<string> ReadLines(string path);
        IAsyncEnumerable<string> ReadLinesAsync(string path);
        void WriteLines(string path, IEnumerable<string> lines);
        Task WriteLinesAsync(string path, IEnumerable<string> lines);
    }
}