using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace App.Services;

public class KeyDbStorage: IStorage
{
    private const string Key = "queries";
    
    private readonly IDatabase _db;

    public KeyDbStorage(IOptions<KeyDbSettings> keyDbSettings)
    {
        var connectionOptions = new ConfigurationOptions
        {
            EndPoints = { keyDbSettings.Value.Endpoint }
        };
        
        var muxer = ConnectionMultiplexer.Connect(connectionOptions);
        _db = muxer.GetDatabase();
    }

    public async Task AddAsync(string query)
    {
        await _db.SortedSetIncrementAsync(Key, query, 1);
    }

    public async Task<IReadOnlyCollection<QueryStat>> GetTopQueries()
    {
        var items = await _db.SortedSetRangeByScoreWithScoresAsync(Key, order: Order.Descending, take: 5);

        return items
            .Select(x => new QueryStat(x.Element.ToString(), Scores: x.Score))
            .ToArray();
    }
}