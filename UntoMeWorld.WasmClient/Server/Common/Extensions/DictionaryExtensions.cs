namespace UntoMeWorld.WasmClient.Server.Common.Extensions;

public static class DictionaryExtensions
{
    public static bool ContainsAnyKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params TKey[] keys)
    {
        return keys.Any(dictionary.ContainsKey);
    }
    public static bool ContainsAnyKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
    {
        return keys.Any(dictionary.ContainsKey);
    }
    public static TValue GetAnyOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params TKey[] keys)
    {
        foreach (var key in keys)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
        }
        return default;
    }
    public static TValue GetAnyOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
    {
        foreach (var key in keys)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
        }
        return default;
    }
}