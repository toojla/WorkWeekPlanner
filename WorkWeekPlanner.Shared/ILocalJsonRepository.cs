namespace WorkWeekPlanner.Shared
{
    public interface ILocalJsonRepository
    {
        void Delete(string fileName);

        IEnumerable<string> ListFiles();

        Task<T?> ReadAsync<T>(string fileName) where T : class;

        Task SaveAsync<T>(string fileName, T obj) where T : class;
    }
}