using System.Collections.Generic;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Stores
{
    public interface ISecurityStore<T> where T : IModel
    {
        public Task<List<T>> All();
        public Task<T> Add(T item);
        public Task<IEnumerable<T>> Add(IEnumerable<T> items);
        public Task<T> Get(string id);
        public Task Update(params T[] items);
        public Task Delete(params string[] ids);   
    }
}