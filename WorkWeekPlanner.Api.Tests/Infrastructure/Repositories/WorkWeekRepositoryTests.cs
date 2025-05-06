using FluentAssertions;
using WorkWeekPlanner.Api.Features.Planner.Models;
using WorkWeekPlanner.Api.Features.Planner.Services;
using WorkWeekPlanner.Api.Infrastructure.Repositories;

namespace WorkWeekPlanner.Api.Tests.Infrastructure.Repositories
{
    public class WorkWeekRepositoryTests
    {
        private readonly string _testDirectory;
        private readonly IWorkWeekRepository _sut;
        private readonly IWorkWeekFactory _workWeekFactory;

        public WorkWeekRepositoryTests()
        {
            // Set up a temporary directory for testing
            _testDirectory = Path.Combine(Path.GetTempPath(), "WorkWeekRepositoryTests");
            if (!Directory.Exists(_testDirectory))
            {
                Directory.CreateDirectory(_testDirectory);
            }
            _sut = new WorkWeekRepository(_testDirectory);
            _workWeekFactory = new WorkWeekFactory(_sut);
        }

        [Fact]
        public async Task SaveAsync_ShouldCreateWorkWeekFile()
        {
            // Arrange
            //var workWeek = new WorkWeek(new DateTime(2025, 5, 6)); // Example date
            var workWeek = await _workWeekFactory.GetOrCreateAsync(new DateTime(2025, 5, 6));

            // Act
            await _sut.SaveAsync(workWeek);
            var filePath = Path.Combine(_testDirectory, workWeek.Year.ToString(), $"Week-{workWeek.WeekNumber}", $"{workWeek.Id}.json");
            var actual = File.Exists(filePath);

            // Assert
            actual.Should().BeTrue();
            RemoveTestArtifacts();
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnWorkWeekObject()
        {
            // Arrange
            //var workWeek = new WorkWeek(new DateTime(2025, 5, 6));
            var workWeek = await _workWeekFactory.GetOrCreateAsync(new DateTime(2025, 5, 6));
            await _sut.SaveAsync(workWeek);

            // Act
            var actual = await _sut.ReadAsync(workWeek.Year, workWeek.WeekNumber);

            // Assert
            actual.Should().NotBeNull();
            actual!.Id.Should().Be(workWeek.Id);
            actual.Year.Should().Be(workWeek.Year);
            actual.WeekNumber.Should().Be(workWeek.WeekNumber);
            RemoveTestArtifacts();
        }

        [Fact]
        public void Delete_ShouldRemoveWorkWeekDirectory()
        {
            // Arrange
            //var workWeek = new WorkWeek(new DateTime(2025, 5, 6));
            var workWeek = _workWeekFactory.GetOrCreateAsync(new DateTime(2025, 5, 6)).Result;
            _sut.SaveAsync(workWeek).Wait();
            var weekDirectory = Path.Combine(_testDirectory, workWeek.Year.ToString(), $"Week-{workWeek.WeekNumber}");

            // Act
            _sut.Delete(workWeek.Year, workWeek.WeekNumber);
            var actual = Directory.Exists(weekDirectory);

            // Assert
            actual.Should().BeFalse();
            RemoveTestArtifacts();
        }

        [Fact]
        public void ListAllWorkWeeks_ShouldReturnAllWorkWeekFiles()
        {
            // Arrange
            //var workWeek1 = new WorkWeek(new DateTime(2025, 5, 6));
            //var workWeek2 = new WorkWeek(new DateTime(2025, 5, 13));
            var workWeek1 = _workWeekFactory.GetOrCreateAsync(new DateTime(2025, 5, 6)).Result;
            var workWeek2 = _workWeekFactory.GetOrCreateAsync(new DateTime(2025, 5, 13)).Result;
            _sut.SaveAsync(workWeek1).Wait();
            _sut.SaveAsync(workWeek2).Wait();

            // Act
            var actual = _sut.ListAllWorkWeeks();

            // Assert
            actual.Should().HaveCount(2);
            RemoveTestArtifacts();
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnNull_WhenFileDoesNotExist()
        {
            // Act
            var actual = await _sut.ReadAsync(2025, 99); // Non-existent week

            // Assert
            actual.Should().BeNull();
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
}