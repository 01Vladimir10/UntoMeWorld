namespace UntoMeWorld.Application.Extensions;

public static class TaskExtensions
{
    public static Task Await<T>(this Task<T>? task) => task ?? Task.CompletedTask;
}