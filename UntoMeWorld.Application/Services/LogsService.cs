using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Application.Services;

public class LogsService : ILogsService
{
    private static readonly Queue<ActionLog> CachedLogs = new();
    private readonly IAuthStateProviderService _auth;
    private readonly IActionLogsStore _logsStore;
    private const int MaxLogsCached = 5;

    public LogsService(IActionLogsStore logsStore, IAuthStateProviderService auth)
    {
        _logsStore = logsStore;
        _auth = auth;
    }

    private void Log(ActionLogLevel level, ActionLogType type, string store, string objectId, object? details = null)
    {
        var currentUser = _auth.CurrentUser;
        CachedLogs.Enqueue(new ActionLog
        {
            CreatedOn = DateTime.Now,
            LastUpdatedOn = DateTime.Now,
            Action = type,
            Level = level,
            ObjectId = objectId,
            ObjectStore = store,
            AuthMethod = _auth.IsTokenAuthenticated ? "Token" : "SigIn",
            UserId = currentUser?.Id ?? "Anonymous",
            Username = currentUser?.Email ?? "Anonymous",
            IpAddress = _auth.RequestIpAddress,
            Details = details
        });
        if (CachedLogs.Count <= MaxLogsCached) return;
        var logsToAdd = CachedLogs.ToList();
        CachedLogs.Clear();
        Task.Run(() => _logsStore.AddMany(logsToAdd));
    }

    public void LogError(ActionLogType type, string store, string objectId, Exception exception,
        object? properties = null)
        => Log(ActionLogLevel.Error, type, store, objectId, new
        {
            Error = exception.ToString(),
            Properties = properties ?? new object()
        });

    public void LogInfo(ActionLogType type, string store, string objectId, object? properties = null)
        => Log(ActionLogLevel.Info, type, store, objectId, properties);

    public void LogDebug(ActionLogType type, string store, string objectId, object? properties = null)
        => Log(ActionLogLevel.Debug, type, store, objectId, properties);


    private async Task<T> ExecuteAndLog<T>(ActionLogType type, string collection, string objectId,
        object? additionalProperties,
        Func<Task<T>> action)
    {
        try
        {
            var result = await action();
            LogInfo(type, collection, objectId, additionalProperties);
            return result;
        }
        catch (Exception e)
        {
            LogError(type, collection, objectId, e);
            throw;
        }
    }

    public async Task ExecuteAndLog(ActionLogType type, string collection, string objectId,
        Func<Task> action)
    {
        try
        {
            await action();
            LogInfo(type, collection, objectId);
        }
        catch (Exception e)
        {
            LogError(type, collection, objectId, e);
            throw;
        }
    }

    public async Task<T> ExecuteAndLogInsertion<T>(string store,
        Func<Task<T>> action) where T : IModel
    {
        try
        {
            var result = await action();
            LogInfo(ActionLogType.Add, store, result.Id);
            return result;
        }
        catch (Exception e)
        {
            LogError(ActionLogType.Add, store, string.Empty, e);
            throw;
        }
    }

    public Task<T> ExecuteAndLog<T>(ActionLogType type, string collection, string objectId,
        Func<Task<T>> action)
        => ExecuteAndLog(type, collection, objectId, null, action);
    
    public Task<IEnumerable<T>> ExecuteAndLog<T>(ActionLogType type, string collection, IEnumerable<T> objects,
        Func<Task<IEnumerable<T>>> action) where T : IModel
        => ExecuteAndLog(type, collection, string.Join(",", objects.Select(o => o.Id)), null, action);

    public Task ExecuteAndLog(ActionLogType type, string collection, IEnumerable<string> objectIds,
        Func<Task> action)
        => ExecuteAndLog(type, collection, string.Join(",", objectIds), action);


    public Task<T> ExecuteAndLog<T>(ActionLogType type, string collection, object additionalProperties,
        Func<Task<T>> action)
        => ExecuteAndLog(type, collection, string.Empty, additionalProperties, action);

    public async Task<IEnumerable<T>> ExecuteAndLogInsertion<T>(string store,
        Func<Task<IEnumerable<T>>> action) where T : IModel
    {
        try
        {
            var result = await action().ContinueWith(t => t.Result.ToList());
            LogInfo(ActionLogType.BulkAdd, store, string.Join(",", result.Select(r => r.Id)));
            return result;
        }
        catch (Exception e)
        {
            LogError(ActionLogType.BulkAdd, store, string.Empty, e);
            throw;
        }
    }
}