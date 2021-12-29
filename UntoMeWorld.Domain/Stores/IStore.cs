using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Domain.Stores
{
    public interface IStore<T>
    {
        public Task<IEnumerable<T>> All();
        public Task<IEnumerable<T>> All(string query);
        public Task<IEnumerable<T>> All(Predicate<T> query);
        public Task<PaginationResult<T>> Query(IEnumerable<DatabaseQueryParameter> parameters, string orderBy = null, bool orderByDesc = false, int page = 1, int pageSize = 100);
        public Task<T> Add(T data);
        public Task<T> Update(T data);
        public Task Delete(T data);
        public Task Delete(params string[] ids);
        public Task<IEnumerable<T>> Add(List<T> data);
        public Task<IEnumerable<T>> Update(List<T> data);
        public Task Delete(IEnumerable<T> data);
        public Task SoftDelete(params string[] ids);
        public Task PermanentlyDelete(params string[] ids);
        public Task Restore(params string[] ids);
        public Task<T> Get(string id);

    }
}