namespace UntoMeWorld.Application.Extensions;

public static class TaskExtensions
{
    public static Task AwaitVoid<T>(this Task<T>? task) => task ?? Task.CompletedTask;
    public static Task<T> Await<T>(this Task<T>? task) => task ?? Task.FromResult<T?>(default)!;
    public static Task<T> OrElse<T>(this Task<T?> task, T defaultValue) =>
        task.ContinueWith(t => t.Result ?? defaultValue);
}