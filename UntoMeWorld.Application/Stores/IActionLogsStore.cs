using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Stores;

public interface IActionLogsStore
{
    public Task<PaginationResult<ActionLog>> Query(QueryFilter? filter = null, string? textQuery = null, string? orderBy = null, bool orderByDesc = false, int page = 1, int pageSize = 100);
    public Task<ActionLog> AddOne(ActionLog data);
    public Task DeleteOne(string key);
    public Task<IEnumerable<ActionLog>> AddMany(List<ActionLog> data);
    public Task DeleteMany(IEnumerable<string> data);
    public Task<ActionLog?> Get(string id);
    
}