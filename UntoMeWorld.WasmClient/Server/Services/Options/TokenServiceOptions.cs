
namespace UntoMeWorld.WasmClient.Server.Services.Options;

public class TokenServiceOptions
{
    public bool ValidateTokensInDatabase { get; set; }
    public bool EnableCaching { get; set; }
    public int CacheLifeTimeInSeconds { get; set; }
}