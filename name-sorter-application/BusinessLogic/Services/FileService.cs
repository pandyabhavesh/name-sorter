using name_sorter_application.Interface;
using Microsoft.Extensions.Logging;

namespace name_sorter_application.BusinessLogic.Services;

public class FileService(ILogger<FileService> log) : IFileService
{
    /// <summary>
    /// Reads lines from a file and returns them as an enumerable sequence of strings.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public IEnumerable<string> ReadLines(string path)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        if (!File.Exists(path))
            throw new FileNotFoundException("Input file not found.", path);
        
        // File.ReadLines streams the file lazily and keeps memory usage low
        return File.ReadLines(path);
    }

    /// <summary>
    /// Writes the specified lines to a file at the given path, creating the file and its directory if they do not
    /// exist.
    /// </summary>
    /// <remarks>If the file already exists at the specified path, it will be deleted and replaced
    /// with the new content. The method ensures that the directory structure for the specified path is created if
    /// it does not already exist.</remarks>
    /// <param name="path">The file path where the lines will be written. Cannot be null, empty, or whitespace.</param>
    /// <param name="lines">The collection of lines to write to the file. Cannot be null.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="path"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="lines"/> is null.</exception>
    public void WriteLines(string path, IEnumerable<string> lines)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        ArgumentNullException.ThrowIfNull(lines, nameof(lines));
       
        // Ensure the directory exists
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            log.LogDebug($"Created directory: {directory}");
        }

        // Delete the file if it already exists
        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            File.Delete(path);
            log.LogDebug($"File Deleted: {path}");
        }
        // Write lines to the file
        File.WriteAllLines(path, lines);
    }

    /// <summary>
    /// Reads lines from a file asynchronously and returns them as an asynchronous stream. 
    /// Efficient asynchronous line reader (avoids blocking threads)
    /// </summary>
    /// <remarks>This method reads the file line by line to minimize memory usage and avoid blocking
    /// threads. It is suitable for processing large files efficiently. Ensure the file exists and is accessible 
    /// before calling this method.</remarks>
    /// <param name="path">The path to the file to read. Must be a valid file path.</param>
    /// <returns>An asynchronous enumerable sequence of strings, where each string represents a line from the file.</returns>
    public async IAsyncEnumerable<string> ReadLinesAsync(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync() is { } line)
        {
            yield return line;
        }
    }

    /// <summary>
    /// Writes the specified lines to a file at the given path asynchronously, creating the file if it does not
    /// exist.
    /// </summary>
    /// <remarks>If the specified path includes directories that do not exist, they will be created
    /// automatically. If the file already exists, it will be deleted and replaced with the new content.</remarks>
    /// <param name="path">The file path where the lines will be written. Cannot be null, empty, or whitespace.</param>
    /// <param name="lines">The collection of lines to write to the file. Cannot be null.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="path"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="lines"/> is null.</exception>
    public async Task WriteLinesAsync(string path, IEnumerable<string> lines)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));

        if (lines == null)
            throw new ArgumentNullException(nameof(lines), "Lines cannot be null.");

        // Ensure the directory exists
        var directory = Path.GetDirectoryName(path);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // (Re)create the temp file so each RecordCount value gets fresh input.
        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            File.Delete(path);
        }
        // Write lines to the file
        await File.WriteAllLinesAsync(path, lines);
    }
}
