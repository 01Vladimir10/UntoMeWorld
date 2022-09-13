namespace UntoMeWorld.Application.Services.Options;

public class JwtTokenFactoryOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public int DefaultTokenDurationInMinutes { get; set; }
}