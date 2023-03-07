namespace App.Services;

public interface IStorage
{
    Task AddAsync(string query);
    Task<IReadOnlyCollection<QueryStat>> GetTopQueries();
}