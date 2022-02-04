namespace UntoMeWorld.WasmClient.Server.Services.Options;

public class RolesServiceOptions : IServiceCachingOptions
{
    public bool EnableCaching { get; set; }
    public int CacheLifetimeInSeconds { get; set; }
}