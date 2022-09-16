using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Extensions.Security;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Services.Crypto;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Security.Authentication;

#nullable enable
public class AuthStateProviderService : IAuthStateProviderService
{
    private readonly IJwtTokenFactory _tokenFactory;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthStateProviderService(IJwtTokenFactory tokenFactory, IHttpContextAccessor httpContextAccessor)
    {
        _tokenFactory = tokenFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => CurrentUser != null;

    public bool IsTokenAuthenticated =>
        _httpContextAccessor.HttpContext?.Request.Headers[ServerConstants.AuthHeaderToken] is not null;

    public bool IsUserAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string? RequestIpAddress =>
        string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"]) 
            ? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
            : _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"];

    public AppUser? CurrentUser
    {
        get
        {
            if (_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
            {
                return _httpContextAccessor.HttpContext?.User.ToAppUser();
            }

            var token = _httpContextAccessor.HttpContext?.Request.Headers[ServerConstants.AuthHeaderToken];
            return !string.IsNullOrEmpty(token) && _tokenFactory.ValidateToken(token)
                ? _tokenFactory.ReadToken(token)?.Claims.ToAppUser()
                : null;
        }
    }
}