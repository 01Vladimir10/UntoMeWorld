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
        public Task<T> Add(T data);
        public Task<T> Update(T data);
        public Task Delete(T data);
        public Task<IEnumerable<T>> Add(List<T> data);
        public Task<IEnumerable<T>> Update(List<T> data);
        public Task Delete(IEnumerable<T> data);

    }
}