using MongoDB.Driver;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Infrastructure.Helpers;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores;

public class MongoActionLogsStore : IActionLogsStore
{
    private readonly IMongoCollection<ActionLog> _collection;

    public MongoActionLogsStore(MongoDbService service)
    {
        _collection = service.GetCollection<ActionLog>("actionLogs");
    }
    
    public async Task<PaginationResult<ActionLog>> Query(QueryFilter? filter = null, string? textQuery = null,
        string? orderBy = null, bool orderByDesc = false,
        int page = 1, int pageSize = 100)
    {
        await Task.Yield();
        var (totalItems, result) =
            await _collection.QueryByPageAndSort(filter, textQuery, orderBy ?? string.Empty,
                orderByDesc, page, pageSize);
        await Task.Delay(1);
        return new PaginationResult<ActionLog>
        {
            Result = result.ToList(),
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            TotalItems = totalItems,
            Page = page
        };
    }

    public Task<ActionLog> AddOne(ActionLog data)
        => _collection.InsertOneAsync(data).ContinueWith(_ => data);

    public Task DeleteOne(string key)
        => _collection.DeleteOneAsync(Builders<ActionLog>.Filter.Eq(l => l.Id, key));

    public Task<IEnumerable<ActionLog>> AddMany(List<ActionLog> data)
        => _collection.InsertManyAsync(data).ContinueWith(_ => data.AsEnumerable());

    public Task DeleteMany(IEnumerable<string> data)
        => _collection.DeleteManyAsync(Builders<ActionLog>.Filter.In(l => l.Id, data));

    public Task<ActionLog?> Get(string id)
        => _collection
            .FindAsync(Builders<ActionLog>.Filter.Eq(l => l.Id, id))
            .ContinueWith(t => t.Result.FirstOrDefault() ?? null);
}