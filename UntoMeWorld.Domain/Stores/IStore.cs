using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UntoMeWorld.Domain.Stores
{
    public interface IStore<T>
    {
        public Task<IEnumerable<T>> All();
        public Task<IEnumerable<T>> All(string query);
        public Task<IEnumerable<T>> All(Predicate<T> query);
        public Task<PaginationResult<T>> Query(string query = null, string orderBy = null, bool orderByDesc = false, int page = 1, int pageSize = 100);
        public Task<T> Add(T data);
        public Task<T> Update(T data);
        public Task Delete(T data);
        public Task Delete(params string[] ids);
        public Task<IEnumerable<T>> Add(List<T> data);
        public Task<IEnumerable<T>> Update(List<T> data);
        public Task Delete(IEnumerable<T> data);

    }
}