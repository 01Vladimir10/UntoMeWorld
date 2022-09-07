namespace UntoMeWorld.WasmClient.Client.Utils.Common;

/// <summary>
/// Helps managing multiple tasks that return the same result.
/// It is meant to prevent async methods from executing when another instance
/// of itself is already executing. This is useful to prevent making requests
/// to remote databases multiple times since the method will only
/// be execute the first time, subsequent requests made while the method is
/// still running will await the initial task.
/// 
/// If another request is executed after the method is execution finishes
/// a new task will be created.
/// </summary>
public class SimpleTaskManager
{
    private Task _currentTask;

    public Task ExecuteTask(Func<Task> function)
    {
        if (_currentTask is { IsCompleted: false })
        {
            return _currentTask;
        }
        _currentTask = function();
        return _currentTask;
    }
}

/// <summary>
/// Helps managing multiple tasks that return the same result.
/// It is meant to prevent async methods from executing when another instance
/// of itself is already executing. This is useful to prevent making requests
/// to remote databases multiple times since the method will only
/// be execute the first time, subsequent requests made while the method is
/// still running will await the initial task.
/// 
/// If another request is executed after the method is execution finishes
/// a new task will be created.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SimpleTaskManager<T>
{
    private Task<T> _currentTask;

    public Task<T> ExecuteTask(Func<Task<T>> function)
    {
        if (_currentTask is { IsCompleted: false })
        {
            return _currentTask;
        }
        _currentTask = function();
        return _currentTask;
    }
}