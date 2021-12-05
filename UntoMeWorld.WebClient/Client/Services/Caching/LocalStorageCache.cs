using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace UntoMeWorld.WebClient.Client.Services.Caching
{
    public class LocalStorageCache<TValue, TKey> : ICacheService<TValue>
    {
        public TimeSpan ItemsLifeSpan { get; set; } = TimeSpan.MaxValue;
        public int MaxItems { get; set; } = 10000;
        private int ItemsToRemoveOnRecycle { get; set; } = 100;
        private readonly ILocalStorageService _storage;
        private Func<TValue, TKey> KeySelector { get; }
        private readonly string _collectionName;
        
        public LocalStorageCache(ILocalStorageService storage, Func<TValue, TKey> keySelector, string collectionName = null)
        {
            _storage = storage;
            KeySelector = keySelector;
            _collectionName = collectionName ?? typeof(TValue).Name;
        }
        
        public async Task InitializeCacheAsync()
        {
            var collection = await _storage.GetItemAsync<IDictionary<TKey, StorageEntry<TValue>>>(_collectionName);
            if (collection == null) 
                await _storage.SetItemAsync(_collectionName, new Dictionary<TKey, StorageEntry<TValue>>());
        }
        
        public async Task<IEnumerable<TValue>> GetAll(Func<Task<IEnumerable<TValue>>> callback, bool force = false)
        {
            if (!force)
            {
                var localEntries = await GetAllStorageEntriesAsList();
                if (localEntries?.Any() ?? false)
                    return localEntries;
            }
            var allEntries = await callback();
            var enumerable = allEntries as TValue[] ?? allEntries.ToArray();
            await UpdateAllEntries(enumerable);
            return enumerable;
        }

        public Task<IEnumerable<TValue>> Find(Predicate<TValue> predicate, Func<Task<TValue>> callback)
        {
            throw new NotImplementedException();
        }

        public async Task<TValue> FindFirst(Predicate<TValue> predicate, Func<Task<TValue>> callback)
        {
            var localEntries = await GetAllStorageEntries();
            var localResult = localEntries.Values.ToList().Find(_ => predicate(_.Data));
            
            if (localResult != null && !localResult.IsExpired(ItemsLifeSpan))
                return localResult.Data;
            try
            {
                var result = await callback();
                if (result != null)
                    await UpsertEntry(result);
                return result;
            }
            catch (ServiceUnavailableException)
            {
                if (localResult != null)
                    return localResult.Data;
            }
            return default;
        }
        

        public async Task<TValue> Add(TValue item)
        {
            await UpsertEntry(item);
            _Update(item);
            return item;
        }

        public async Task<TValue> Update(TValue item)
        {
            await UpsertEntry(item);
            _Update(item);
            return item;
        }

        private async void _Update(TValue item)
        {
            var updatedItem = await OnUpdated(item);
            await UpsertEntry(updatedItem);
        }

        public async Task Delete(TValue item)
        {
            await DeleteEntry(item);
            _Delete(item);
        }
        private async void _Delete(TValue value)
        {
            await OnDeleted(value);
        }
        
        private async Task<List<TValue>> GetAllStorageEntriesAsList()
        {
            var entries = await _storage.GetItemAsync<Dictionary<TKey, StorageEntry<TValue>>>(_collectionName);
            return entries?.Select(t => t.Value.Data).ToList();
        }
        private async Task<IDictionary<TKey, StorageEntry<TValue>>> GetAllStorageEntries()
        {
            return await _storage.GetItemAsync<Dictionary<TKey, StorageEntry<TValue>>>(_collectionName);
        }
        private async Task DeleteEntry(TValue value)
        {
            var entries = await GetAllStorageEntries();
            entries.Remove(KeySelector(value));
        }
        private async Task UpsertEntry(params TValue[] newEntries)
        {
            var entries = await GetAllStorageEntries();
            foreach (var entry in newEntries)
            {
                entries[KeySelector(entry)] = new StorageEntry<TValue>(entry);
            }
            
            if (entries.Count > MaxItems)
            {
                var expiredEntries = entries.Where(c => DateTime.Now - c.Value.LastUpdated > ItemsLifeSpan).ToList();
                foreach (var expiredEntry in expiredEntries)
                    entries.Remove(expiredEntry.Key);
                // If we already removed old items but there is not enough space
                if (entries.Count - expiredEntries.Count - ItemsToRemoveOnRecycle >= MaxItems)
                {
                    // Remove the oldest entries
                    var removedEntries = entries.OrderBy(c => c.Value.LastUpdated).Take(ItemsToRemoveOnRecycle);
                    foreach (var removedEntry in removedEntries)
                        entries.Remove(removedEntry.Key);
                }
            }
            await _storage.SetItemAsync(_collectionName, entries);
        }

        private async Task UpdateAllEntries(IEnumerable<TValue> entries)
        {
            var dictionary = entries.ToDictionary(e => KeySelector(e), e => new StorageEntry<TValue>(e));
            await _storage.SetItemAsync(_collectionName, dictionary);
        }
        public Func<TValue, Task<TValue>> OnAdded { get; set; } = Task.FromResult; 
        public Func<TValue, Task<TValue>> OnUpdated { get; set; } = Task.FromResult;
        public Func<TValue, Task> OnDeleted { get; set; } = Task.FromResult;
        public Func<Exception> OnError { get; set; }
    }
    
    internal class StorageEntry<TValue>
    {
        public DateTime LastUpdated { get; } = DateTime.Now;
        public TValue Data { get; }
        public StorageEntry(TValue data)
        {
            Data = data;
        }
        public bool IsExpired(TimeSpan lifespan) => DateTime.Now - LastUpdated > lifespan;
    }
}