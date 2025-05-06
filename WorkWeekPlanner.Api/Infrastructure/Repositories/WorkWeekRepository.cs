using System.Text.Json;
using WorkWeekPlanner.Api.Features.Planner.Models;

namespace WorkWeekPlanner.Api.Infrastructure.Repositories;

public class WorkWeekRepository : IWorkWeekRepository
{
    private readonly string _baseDirectory;

    public WorkWeekRepository(string baseDirectory)
    {
        _baseDirectory = baseDirectory;

        // Ensure the base directory exists
        if (!Directory.Exists(_baseDirectory))
        {
            Directory.CreateDirectory(_baseDirectory);
        }
    }

    public async Task SaveAsync(WorkWeek workWeek)
    {
        try
        {
            if (workWeek == null) throw new ArgumentNullException(nameof(workWeek));

            // Create directory structure based on year and week
            string yearDirectory = Path.Combine(_baseDirectory, workWeek.Year.ToString());
            if (!Directory.Exists(yearDirectory))
            {
                Directory.CreateDirectory(yearDirectory);
            }

            string weekDirectory = Path.Combine(yearDirectory, $"Week-{workWeek.WeekNumber}");
            if (!Directory.Exists(weekDirectory))
            {
                Directory.CreateDirectory(weekDirectory);
            }

            // Save the WorkWeek object as a JSON file
            string filePath = Path.Combine(weekDirectory, $"{workWeek.Id}.json");
            string json = JsonSerializer.Serialize(workWeek, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(filePath, json);
        }
        catch (IOException ex)
        {
            throw new Exception($"An error occurred while saving the WorkWeek '{workWeek.Id}': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"An error occurred while serializing the WorkWeek '{workWeek.Id}': {ex.Message}", ex);
        }
    }

    public async Task<WorkWeek?> ReadAsync(int year, int weekNumber)
    {
        try
        {
            // Construct the file path
            string filePath = Path.Combine(_baseDirectory, year.ToString(), $"Week-{weekNumber}", $"{year}-W{weekNumber}.json");

            if (!File.Exists(filePath)) return null;

            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<WorkWeek>(json);
        }
        catch (IOException ex)
        {
            throw new Exception($"An error occurred while reading the WorkWeek for Year {year}, Week {weekNumber}: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"An error occurred while deserializing the WorkWeek for Year {year}, Week {weekNumber}: {ex.Message}", ex);
        }
    }

    public void Delete(int year, int weekNumber)
    {
        try
        {
            // Construct the directory path
            string weekDirectory = Path.Combine(_baseDirectory, year.ToString(), $"Week-{weekNumber}");

            if (Directory.Exists(weekDirectory))
            {
                Directory.Delete(weekDirectory, true);
            }
        }
        catch (IOException ex)
        {
            throw new Exception($"An error occurred while deleting the WorkWeek for Year {year}, Week {weekNumber}: {ex.Message}", ex);
        }
    }

    public IEnumerable<string> ListAllWorkWeeks()
    {
        try
        {
            if (!Directory.Exists(_baseDirectory))
                return Array.Empty<string>();

            // List all JSON files in the directory structure
            return Directory.GetFiles(_baseDirectory, "*.json", SearchOption.AllDirectories);
        }
        catch (IOException ex)
        {
            throw new Exception($"An error occurred while listing all WorkWeeks: {ex.Message}", ex);
        }
    }
}