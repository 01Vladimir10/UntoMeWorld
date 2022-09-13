namespace UntoMeWorld.WasmClient.Server.Common.Model;

public class CacheEntry<T>
{
    public DateTime LastUpdated { get; } = DateTime.Now;
    
    public T Data { get; set; }
#if DEBUG
    public TimeSpan LifeSpan { get; set; } = TimeSpan.Zero;
#else
    public TimeSpan LifeSpan { get; set; } = TimeSpan.FromMinutes(5);
#endif
    public bool IsExpired => DateTime.Now - LastUpdated > LifeSpan;
}