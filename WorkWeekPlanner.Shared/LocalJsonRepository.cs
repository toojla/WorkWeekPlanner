using System.Text.Json;

namespace WorkWeekPlanner.Shared;

public class LocalJsonRepository : ILocalJsonRepository
{
    private readonly string _directoryPath;

    public LocalJsonRepository(string directoryPath)
    {
        _directoryPath = directoryPath;

        // Ensure the directory exists
        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }
    }

    public void Delete(string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            string filePath = Path.Combine(_directoryPath, $"{fileName}.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (IOException ex)
        {
            // Handle file I/O errors
            throw new Exception($"An error occurred while deleting the file '{fileName}.json': {ex.Message}", ex);
        }
    }

    public IEnumerable<string> ListFiles()
    {
        try
        {
            if (!Directory.Exists(_directoryPath)) return Array.Empty<string>();

            return Directory.GetFiles(_directoryPath, "*.json");
        }
        catch (IOException ex)
        {
            // Handle file I/O errors
            throw new Exception($"An error occurred while listing files in the directory '{_directoryPath}': {ex.Message}", ex);
        }
    }

    public async Task<T?> ReadAsync<T>(string fileName) where T : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            string filePath = Path.Combine(_directoryPath, $"{fileName}.json");

            if (!File.Exists(filePath))
                return null;

            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (IOException ex)
        {
            // Handle file I/O errors
            throw new Exception($"An error occurred while reading the file '{fileName}.json': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            throw new Exception($"An error occurred while deserializing the JSON file '{fileName}.json': {ex.Message}", ex);
        }
    }

    public async Task SaveAsync<T>(string fileName, T obj) where T : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            string filePath = Path.Combine(_directoryPath, $"{fileName}.json");
            string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(filePath, json);
        }
        catch (IOException ex)
        {
            // Handle file I/O errors
            throw new Exception($"An error occurred while saving the file '{fileName}.json': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            // Handle JSON serialization errors
            throw new Exception($"An error occurred while serializing the object to JSON: {ex.Message}", ex);
        }
    }
}