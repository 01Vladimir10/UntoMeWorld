using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class AppUsersService : GenericDatabaseService<AppUser>, IUserService
{
    private readonly ICacheService _cache;
    private const string CachePrefix = "Users__";
    public AppUsersService(IUserStore store, ICacheService cache) : base(store)
    {
        _cache = cache;
    }

    private void CacheItem(IModel user)
    {
        _cache.SetEntry(BuildCacheKey(user), user);
    }

    private static string BuildCacheKey(IModel user)
        => BuildCacheKey(user.Id);
    private static string BuildCacheKey(string id)
        => CachePrefix + id;

    public new async Task<AppUser> Add(AppUser item)
    {
        var createdItem = await base.Add(item);
        CacheItem(createdItem);
        return createdItem;
    }

    public new Task<AppUser> Get(string id)
        => _cache.GetEntry(BuildCacheKey(id), () => base.Get(id));

    public new async Task<AppUser> Update(AppUser item)
    {
        var updatedItem = await base.Update(item);
        CacheItem(updatedItem);
        return updatedItem;
    }

    public new async Task Restore(string item)
    {
        await base.Restore(item);
        var cachedItem = await _cache.Get<AppUser>(BuildCacheKey(item));
        if (cachedItem == null) return;
        cachedItem.IsDeleted = false;
        CacheItem(cachedItem);
    }

    public new async Task Delete(string id, bool softDelete = true)
    {
        await base.Delete(id, softDelete);

        if (!softDelete)
        {
            _cache.SetEntry(BuildCacheKey(id), null);
            return;
        }
        
        var cachedItem = await _cache.Get<AppUser>(BuildCacheKey(id));
        if (cachedItem == null) return;
        cachedItem.IsDeleted = false;
        CacheItem(cachedItem);
    }

    public new async Task<IEnumerable<AppUser>> Add(IEnumerable<AppUser> items)
    {
        var appUsers = items as AppUser[] ?? items.ToArray();
        await base.Add(appUsers);
        foreach (var appUser in appUsers)
            CacheItem(appUser);
        return appUsers;
    }

    public new async Task<IEnumerable<AppUser>> Update(IEnumerable<AppUser> item)
    {
        var updatedItems = await base.Update(item);

        var appUsers = updatedItems as AppUser[] ?? updatedItems.ToArray();
        
        foreach (var updatedItem in appUsers)
            CacheItem(updatedItem);
        return appUsers;
    }

    public new async Task Restore(IEnumerable<string> item)
    {
        var userIds = item as string[] ?? Array.Empty<string>();
        await base.Restore(userIds);
        foreach (var userId in userIds)
        {
            var cachedItem = await _cache.Get<AppUser>(BuildCacheKey(userId));
            if (cachedItem == null) continue;
            cachedItem.IsDeleted = false;
            CacheItem(cachedItem);
        }
    }

    public new async Task Delete(IEnumerable<string> ids, bool softDelete = true)
    {
        var userIds = ids as string[] ?? Array.Empty<string>();
        await base.Delete(userIds, softDelete);

        if (!softDelete)
        {
            foreach (var userId in userIds)
                _cache.SetEntry(BuildCacheKey(userId), null);
            return;
        }
        foreach (var userId in userIds)
        {
            var cachedItem = await _cache.Get<AppUser>(BuildCacheKey(userId));
            if (cachedItem == null) continue;
            cachedItem.IsDeleted = false;
            CacheItem(cachedItem);
        }
    }
}