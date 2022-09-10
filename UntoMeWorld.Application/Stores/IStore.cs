using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Common;

namespace UntoMeWorld.Application.Stores
{
    public interface IStore<T>
    {
        public Task<IEnumerable<T>> All();
        public Task<IEnumerable<T>> All(string query);
        public Task<IEnumerable<T>> All(Predicate<T> query);
        public Task<PaginationResult<T>> Query(QueryFilter filter = null, string textQuery = "", string orderBy = null, bool orderByDesc = false, int page = 1, int pageSize = 100);
        public Task<T> AddOne(T data);
        public Task<T> UpdateOne(T data);
        public Task DeleteOne(string key);
        public Task PurgeOne(string key);
        public Task RestoreOne(string key);
        public Task<IEnumerable<T>> AddMany(List<T> data);
        public Task<IEnumerable<T>> UpdateMany(List<T> data);
        public Task DeleteMany(IEnumerable<string> data);
        public Task PurgeMany(IEnumerable<string> data);
        public Task RestoreMany(IEnumerable<string> keys);
        public Task<T> Get(string id);

    }
}