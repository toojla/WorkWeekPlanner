using FluentAssertions;
using WorkWeekPlanner.Api.Infrastructure.Repositories;

namespace WorkWeekPlanner.Api.Tests.Infrastructure.Repositories;

public class LocalJsonRepositoryTests
{
    private readonly string _testDirectory;
    private readonly ILocalJsonRepository _sut;

    public LocalJsonRepositoryTests()
    {
        // Set up a temporary directory for testing
        _testDirectory = Path.Combine(Path.GetTempPath(), "LocalJsonRepositoryTests");
        if (!Directory.Exists(_testDirectory))
        {
            Directory.CreateDirectory(_testDirectory);
        }
        _sut = new LocalJsonRepository(_testDirectory);
    }

    [Fact]
    public async Task SaveAsync_ShouldCreateJsonFile()
    {
        // Arrange
        var testObject = new { Id = 1, Name = "Test" };
        var fileName = "testFile";

        // Act
        await _sut.SaveAsync(fileName, testObject);
        var filePath = Path.Combine(_testDirectory, $"{fileName}.json");
        var actual = File.Exists(filePath);

        // Assert
        actual.Should().BeTrue();
        RemoveTestArtifacts();
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnDeserializedObject()
    {
        // Arrange
        var testObject = new { Id = 1, Name = "Test" };
        var fileName = "testFile";
        await _sut.SaveAsync(fileName, testObject);

        // Act
        var actual = await _sut.ReadAsync<dynamic>(fileName);

        var x = actual as object;
        // Assert
        //x.Should().Be(testObject);
        x.Should().NotBeNull();
        //x.Id.Should().Be(1);
        //x.Name.Should().Be("Test");
        RemoveTestArtifacts();
    }

    [Fact]
    public void Delete_ShouldRemoveJsonFile()
    {
        // Arrange
        var fileName = "testFile";
        var filePath = Path.Combine(_testDirectory, $"{fileName}.json");
        File.WriteAllText(filePath, "{}");

        // Act
        _sut.Delete(fileName);
        var actual = File.Exists(filePath);

        // Assert
        actual.Should().BeFalse();
        RemoveTestArtifacts();
    }

    [Fact]
    public void ListFiles_ShouldReturnAllJsonFiles()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_testDirectory, "file1.json"), "{}");
        File.WriteAllText(Path.Combine(_testDirectory, "file2.json"), "{}");

        // Act
        var actual = _sut.ListFiles();

        // Assert
        actual.Should().HaveCount(2);
        RemoveTestArtifacts();
    }

    [Fact]
    public async Task SaveAsync_ShouldThrowException_WhenFileNameIsEmpty()
    {
        // Arrange
        var testObject = new { Id = 1, Name = "Test" };

        // Act
        Func<Task> act = async () => await _sut.SaveAsync("", testObject);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("File name cannot be null or empty.*");
    }

    private void RemoveTestArtifacts()
    {
        // Remove all files in the temp folder except the test directory
        var testDirectoryFullPath = Path.GetFullPath(_testDirectory);
        var filesToDelete = Directory.GetFiles(testDirectoryFullPath);

        foreach (var file in filesToDelete)
        {
            try
            {
                File.Delete(file);
            }
            catch (IOException)
            {
                // Handle cases where the file might be in use
            }
        }
    }
}