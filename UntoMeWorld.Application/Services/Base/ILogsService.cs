using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Application.Services.Base;

public interface ILogsService
{
    void LogError(ActionLogType type, string store, string objectId, Exception exception,
        object? properties = null);

    void LogInfo(ActionLogType type, string store, string objectId, object? properties = null);
    void LogDebug(ActionLogType type, string store, string objectId, object? properties = null);

    Task ExecuteAndLog(ActionLogType type, string collection, string objectId,
        Func<Task> action);

    Task<T> ExecuteAndLog<T>(ActionLogType type, string collection, string objectId,
        Func<Task<T>> action);

    Task<IEnumerable<T>> ExecuteAndLog<T>(ActionLogType type, string collection, IEnumerable<T> objects,
        Func<Task<IEnumerable<T>>> action) where T : IModel;

    Task ExecuteAndLog(ActionLogType type, string collection, IEnumerable<string> objectIds,
        Func<Task> action);

    Task<T> ExecuteAndLog<T>(ActionLogType type, string collection, object additionalProperties,
        Func<Task<T>> action);

    Task<T> ExecuteAndLogInsertion<T>(string store,
        Func<Task<T>> action) where T : IModel;

    Task<IEnumerable<T>> ExecuteAndLogInsertion<T>(string store,
        Func<Task<IEnumerable<T>>> action) where T : IModel;
}