namespace WorkWeekPlanner.Shared
{
    public interface ILocalJsonRepository
    {
        Task SaveAsync<T>(string fileName, T obj) where T : class;
        Task<T?> ReadAsync<T>(string fileName) where T : class;
        void Delete(string fileName);
        IEnumerable<string> ListFiles();
    }
}
