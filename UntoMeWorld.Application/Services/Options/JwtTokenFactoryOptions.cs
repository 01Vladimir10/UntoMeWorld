namespace UntoMeWorld.Application.Services.Options;

public class JwtTokenFactoryOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int DefaultTokenDurationInMinutes { get; set; }
}