using Microsoft.Extensions.Logging;
using Moq;
using name_sorter_application.BusinessLogic.Services;
using name_sorter_application.Interface;

namespace name_sorter_test
{
    public class PersonServiceTests
    {
        [Fact]
        public void Load_ShouldReturnEmptyList_WhenFileIsEmpty()
        {
            // Arrange
            var fileService = new Mock<IFileService>();
            fileService.Setup(fs => fs.ReadLines(It.IsAny<string>())).Returns(Enumerable.Empty<string>());
            var parser = new Mock<IPersonParser>();
            var logger = new Mock<ILogger<PersonService>>();
            var service = new PersonService(fileService.Object, parser.Object, logger.Object);
            // Act
            var result = service.Load("empty.txt");
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Load_ShouldReturnList_WhenFileHasContent()
        {
            var recordCount = 1000;
            var fileLines = new List<string>();
            for (int i = 0; i < recordCount; i++)
            {
                fileLines.Add(GenerateRandomName());
            }

            // Arrange
            var fileService = new Mock<IFileService>();
            fileService.Setup(fs => fs.ReadLines(It.IsAny<string>())).Returns(fileLines);
            var parser = new Mock<IPersonParser>();
            var logger = new Mock<ILogger<PersonService>>();
            var service = new PersonService(fileService.Object, parser.Object, logger.Object);
            // Act
            var result = service.Load("file.txt");
            // Assert
            Assert.True(result.Count == recordCount);
        }

        private string GenerateRandomName()
        {
            Random _rand = new(42);

            var firstNames = new[] {
                    "Olivia", "Liam", "Charlotte", "Noah", "Amelia", "Oliver",
                    "Isla", "Leo", "Ava", "Lucas", "Mia", "Ethan", "Sam", "Bob", "Xi"
            };

            var middleNames = new[]
            {
                "Grace", "James", "Rose", "Alexander", "Jane", "Michael",
                "Louise", "Matthew", "Claire", "Thomas", "May", "John M", "Tom Johnson"
            };

            var lastNames = new[] {
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia",
                "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Patel", "Chen", "Zhao"
            };

            string first = firstNames[_rand.Next(firstNames.Length)];
            string last = lastNames[_rand.Next(lastNames.Length)];

            // Roughly 50 % of records get a middle name
            if (_rand.NextDouble() < 0.5)
            {
                string middle = middleNames[_rand.Next(middleNames.Length)];
                return $"{first} {middle} {last}";
            }

            return $"{first} {last}";
        }
    }
}