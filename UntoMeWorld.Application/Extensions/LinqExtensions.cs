namespace UntoMeWorld.Application.Extensions;

public static class LinqExtensions
{
    public static TItem GetOr<TKey, TItem>(this IDictionary<TKey, TItem> source, TKey key, TItem defaultValue)
        => source.ContainsKey(key) ? source[key] : defaultValue;

    public static TItem? GetOrDefault<TKey, TItem>(this IDictionary<TKey, TItem> source, TKey key)
        => source.ContainsKey(key) ? source[key] : default;

}