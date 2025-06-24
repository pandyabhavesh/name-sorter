# name-sorter solution
# description: A benchmark-ready .NET console application to read, parse, and sort large person data file using modern C# and Serilog.

## Projects
- `name-sorter`: The main console application that orchastrates reads, parses, and sorts the person data file.
- `name-sorter.application`: Contains the mail logic for file loading, parsing, and sorting logic.
- `name-sorter.Tests`: Unit tests for the `name-sorter.application` application.
- `name-sorter.Benchmark`: Benchmark tests for performance evaluation of the sorting algorithm.

## Features
- Dependency Injection (Microsoft.Extensions.DependencyInjection)
- Serilog Logging (via appsettings.json)
- BenchmarkDotNet for performance testing
- Parallel sorting and LINQ performance comparison
- Unit tests using xUnit and Moq for mocking

## How to run
- Ensure you have .NET SDK installed (8.0).
- Clone the repository and navigate to the `name-sorter` directory.
- path-to-person-data-file is optional, if not provided, it will default to `unsorted-names-list.txt` in the current directory.
- output-file-path is optional, if not provided, it will default to `sorted-names-list.txt` in the current directory.
- Run the application using the command:
  ```bash
	dotnet build
	dotnet run --project name-sorter <path-to-person-data-file> <output-file-path>
  ```
