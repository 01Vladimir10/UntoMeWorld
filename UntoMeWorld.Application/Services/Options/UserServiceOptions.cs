namespace UntoMeWorld.Application.Services.Options;

public class UserServiceOptions : IServiceCachingOptions
{
    public bool EnableCaching { get; set; }
    public int CacheLifetimeInSeconds { get; set; }
}