
namespace UntoMeWorld.Application.Services.Options;

public class TokenServiceOptions : IServiceCachingOptions
{
    public bool ValidateTokensInDatabase { get; set; }
    public bool EnableCaching { get; set; }
    public int CacheLifetimeInSeconds { get; set; }
}