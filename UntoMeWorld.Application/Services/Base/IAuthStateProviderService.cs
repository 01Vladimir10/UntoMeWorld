using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Services.Base;

public interface IAuthStateProviderService
{
    public bool IsAuthenticated { get; }
    public bool IsTokenAuthenticated  { get; }
    public bool IsUserAuthenticated  { get; }
    
    public string? RequestIpAddress { get; }
    public AppUser? CurrentUser  { get; }
}