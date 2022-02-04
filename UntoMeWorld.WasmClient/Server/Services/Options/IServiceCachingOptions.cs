namespace UntoMeWorld.WasmClient.Server.Services.Options;

public interface IServiceCachingOptions
{
    public bool EnableCaching { get; set; }
    public int CacheLifetimeInSeconds { get; set; }
}