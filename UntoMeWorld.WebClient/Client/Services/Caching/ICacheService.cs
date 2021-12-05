#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UntoMeWorld.WebClient.Client.Services.Caching
{
    public interface ICacheService<T>
    {
        public TimeSpan ItemsLifeSpan { get; set; }
        public int MaxItems { get; set; }
        public Task InitializeCacheAsync();
        public Task<IEnumerable<T>> GetAll(Func<Task<IEnumerable<T>>> callback, bool force = false);
        public Task<IEnumerable<T>> Find(Predicate<T> predicate, Func<Task<T?>> callback);
        public Task<T> FindFirst(Predicate<T> predicate, Func<Task<T?>> callback);
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task Delete(T item);
        public Func<T, Task<T>> OnAdded { get; set; }
        public Func<T, Task<T>> OnUpdated { get; set; }
        public Func<T, Task> OnDeleted { get; set; }
    }

    public class ServiceUnavailableException : Exception
    {
    }
}