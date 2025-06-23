using name_sorter_application.BusinessLogic.Helper;
using name_sorter_application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace name_sorter_test
{
    public class PersonParserTests
    {
        [Fact]
        public void ParseLine_ShouldThrowFormatException_WhenLineIsEmpty()
        {
            // Arrange
            var parser = new PersonParser();
            string line = string.Empty;
            // Act & Assert
            Assert.Throws<FormatException>(() => parser.ParseLine(line));
        }

        [Fact]
        public void ParseLine_ShouldThrowFormatException_WhenLineHasOnlyOnePart()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John"; // Only first name, no last name
            // Act & Assert
            Assert.Throws<FormatException>(() => parser.ParseLine(line));
        }

        [Fact]
        public void ParseLine_ShouldReturnPerson_WhenLineHasFirstAndLastName()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John Doe"; // Valid first and last name
            // Act
            var person = parser.ParseLine(line);
            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Null(person.MiddleNames); // No middle names
            Assert.Equal("Doe", person.LastName);
        }

        [Fact] 
        public void ParseLine_ShouldReturnPerson_WhenLineHasMiddleNames()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John Michael Doe"; // Valid first, middle, and last name
            // Act
            var person = parser.ParseLine(line);
            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Michael", person.MiddleNames); // Middle names should be parsed correctly
            Assert.Equal("Doe", person.LastName);
        }

        [Fact]
        public void FormatLine_ShouldReturnFormattedString_WhenPersonHasMiddleNames()
        {
            // Arrange
            var parser = new PersonParser();
            var person = new Person("John", "Michael", "Doe");
            // Act
            string formattedLine = parser.FormatLine(person);
            // Assert
            Assert.Equal("John Michael Doe", formattedLine);
        }


        [Fact]
        public void FormatLine_ShouldReturnFormattedString_WhenPersonHasTwoMiddleNames()
        {
            // Arrange
            var parser = new PersonParser();
            var person = new Person("John", "Michael Second", "Doe");
            // Act
            string formattedLine = parser.FormatLine(person);
            // Assert
            Assert.Equal("John Michael Second Doe", formattedLine);
        }

        [Fact]
        public void FormatLine_ShouldReturnFormattedString_WhenPersonHasNoMiddleNames()
        {
            // Arrange
            var parser = new PersonParser();
            var person = new Person("John", null, "Doe"); // No middle names
            // Act
            string formattedLine = parser.FormatLine(person);
            // Assert
            Assert.Equal("John Doe", formattedLine);
        }

        [Fact]
        public void FormatLine_ShouldReturnFormattedString_WhenPersonHasEmptyMiddleNames()
        {
            // Arrange
            var parser = new PersonParser();
            var person = new Person("John", string.Empty, "Doe"); // Empty middle names
            // Act
            string formattedLine = parser.FormatLine(person);
            // Assert
            Assert.Equal("John Doe", formattedLine); // Should handle empty middle names gracefully
        }

        
        [Fact]
        public void ParseLine_ShouldHandleMultipleSpacesBetweenNames()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John   Michael    Doe"; // Multiple spaces between names
            // Act
            var person = parser.ParseLine(line);
            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Michael", person.MiddleNames); // Middle names should be parsed correctly
            Assert.Equal("Doe", person.LastName);
        }

        [Fact]
        public void ParseLine_ShouldHandleLeadingAndTrailingSpaces()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "   John Michael Doe   "; // Leading and trailing spaces
            // Act
            var person = parser.ParseLine(line);
            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Michael", person.MiddleNames); // Middle names should be parsed correctly
            Assert.Equal("Doe", person.LastName);
        }

        [Fact]
        public void ParseLine_ShouldHandleSingleNameWithSpaces()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John   "; // Single name with trailing spaces
            // Act & Assert
            Assert.Throws<FormatException>(() => parser.ParseLine(line)); // Should throw because it lacks a last name
        }

        [Fact]
        public void ParseLine_ShouldHandleMultipleNamesWithSpaces()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John   Michael   Doe"; // Multiple spaces between names
            // Act
            var person = parser.ParseLine(line);
            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Michael", person.MiddleNames); // Middle names should be parsed correctly
            Assert.Equal("Doe", person.LastName);
        }

        [Fact]
        public void ParseLine_ShouldHandleNamesWithSpecialCharacters()
        {
            // Arrange
            var parser = new PersonParser();
            string line = "John O'Connor Doe"; // Name with apostrophe
            // Act
            var person = parser.ParseLine(line);
            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("O'Connor", person.MiddleNames); // No middle names should include the apostrophe
            Assert.Equal("Doe", person.LastName); // Last name 
        }
    }
}
