using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UntoMeWorld.Domain.Stores
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<IEnumerable<T>> GetByQuery(string query);
        public Task<IEnumerable<T>> GetByQuery(Predicate<T> query);
        public Task<T> Insert(T data);
        public Task<T> Modify(T data);
        public Task Remove(T data);
        public Task<IEnumerable<T>> Insert(IEnumerable<T> data);
        public Task<IEnumerable<T>> Modify(IEnumerable<T> data);
        public Task Remove(IEnumerable<T> data);

    }
}