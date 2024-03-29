﻿using UntoMeWorld.Application.Helpers;

namespace UntoMeWorld.Application.Services.Base;

public interface ICacheService : IDisposable
{
    public Task<T> GetEntry<T>(string key, Func<Task<T>> callback);
    public Task<T> GetEntry<T>(string key, Func<Task<CacheEntry<T>>> callback);
    public void SetEntry(string key, object value);
    public void SetEntry<T>(string key, CacheEntry<T> entry);
    public Task<T> Get<T>(string key);
}