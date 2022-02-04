using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Helpers;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Options;

namespace UntoMeWorld.WasmClient.Server.Services;

public class AppUsersService : GenericDatabaseService<AppUser>, IUserService
{
    private const string CachePrefix = "Users__";
    private readonly CacheHelper<AppUser, string> _cache;
    private readonly bool _enableCache;
    private readonly IUserStore _userStore;

    public AppUsersService(IUserStore store, IMemoryCache cache, IOptions<UserServiceOptions> options) : base(store)
    {
        _userStore = store;
        _enableCache = options.Value?.EnableCaching ?? false;
        _cache = new CacheHelper<AppUser, string>(cache, CachePrefix, TimeSpan.FromSeconds(options.Value?.CacheLifetimeInSeconds ?? 0));
    }

    public new async Task<AppUser> Add(AppUser item)
    {
        var createdItem = await base.Add(item);
        _cache.Set(createdItem.Id, createdItem);
        return createdItem;
    }

    public new Task<AppUser> Get(string id)
        => _enableCache
            ? _cache.Get(id, () => base.Get(id))
            : base.Get(id);


    public new async Task<AppUser> Update(AppUser item)
    {
        var updatedItem = await base.Update(item);
        if (_enableCache)
            _cache.Set(updatedItem.Id, updatedItem);
        return updatedItem;
    }

    public new async Task Restore(string item)
    {
        await base.Restore(item);
        if (_enableCache)
            _cache.UpdateIfPresent(item, user =>
            {
                user.IsDeleted = false;
                return user;
            });
    }

    public new async Task Delete(string id, bool softDelete = true)
    {
        await base.Delete(id, softDelete);

        if (!_enableCache)
            return;

        if (softDelete)
            _cache.UpdateIfPresent(id, user =>
            {
                user.IsDeleted = true;
                return user;
            });
        else
            _cache.InvalidateItem(id);
    }

    public new async Task<IEnumerable<AppUser>> Add(IEnumerable<AppUser> items)
    {
        var appUsers = items as AppUser[] ?? items.ToArray();
        await base.Add(appUsers);

        if (_enableCache)
            _cache.Set(user => user.Id, appUsers);

        return appUsers;
    }

    public new async Task<IEnumerable<AppUser>> Update(IEnumerable<AppUser> item)
    {
        var updatedItems = await base.Update(item);
        var appUsers = updatedItems as AppUser[] ?? updatedItems.ToArray();

        if (_enableCache)
            _cache.Set(user => user.Id, appUsers);
        return appUsers;
    }

    public new async Task Restore(IEnumerable<string> item)
    {
        var userIds = item as string[] ?? Array.Empty<string>();
        await base.Restore(userIds);
        if (_enableCache)
            _cache.UpdateIfPresent((user, _) =>
            {
                user.IsDeleted = false;
                return user;
            }, userIds);
    }

    public new async Task Delete(IEnumerable<string> ids, bool softDelete = true)
    {
        var userIds = ids as string[] ?? Array.Empty<string>();
        await base.Delete(userIds, softDelete);

        if (!_enableCache)
            return;
        if (!softDelete)
            _cache.InvalidateItem(userIds);
        else
            _cache.UpdateIfPresent((user, _) =>
            {
                user.IsDeleted = true;
                return user;
            }, userIds);
    }

    private static string BuildThirdPartyAuthenticatedUserId(string authenticationProvider, string thirdPartyUserId)
        => authenticationProvider + "__" + thirdPartyUserId;

    public async Task<AppUser> GetOrCreateUserByThirdPartyAccountInfo(string provider, string thirdPartyUserId,
        Func<AppUser> onCreateCallback)
    {
        var user =
            await _userStore.GetByThirdPartyUserId(provider, thirdPartyUserId);
        if (user != null)
            return user;
        var userInfo = onCreateCallback();
        userInfo.AuthProviderUserId = thirdPartyUserId;
        userInfo.AuthProvider = provider;
        userInfo.Roles = new List<string> { "default" };
        return await Store.Add(userInfo);
    }

    public async Task<bool> IsDisabled(string userId)
    {
        var user = _enableCache ? 
            await _cache.Get(userId, () => _userStore.Get(userId)) :
            await _userStore.Get(userId);
        return (user?.IsDisabled ?? true) || user.IsDeleted;
    }


    public Task<AppUser> GetOrCreateUserByThirdPartyAccountInfo(IEnumerable<Claim> claims)
    {
        throw new NotImplementedException();
    }
}