using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization;

public class ApiAuthorizationHandler : AuthorizationHandler<ApiAuthorizationRequirement>
{
    private readonly IApiAuthorizationService _authorization;
    private readonly IHttpContextAccessor _contextAccessor;
    
    public ApiAuthorizationHandler(IApiAuthorizationService authorization, IHttpContextAccessor contextAccessor)
    {
        _authorization = authorization;
        _contextAccessor = contextAccessor;
    }
    private HttpContext HttpContext => _contextAccessor.HttpContext;
    private HttpRequest Request => HttpContext?.Request;
    private string RequestController => Request?.RouteValues["controller"]?.ToString() ?? string.Empty;
    private string RequestAction => Request?.RouteValues["action"]?.ToString() ?? string.Empty;
    private bool IsUserAuthenticated => HttpContext?.User.Identity?.IsAuthenticated ?? false;
    private bool IsTokenAuthenticated => (Request?.Headers.ContainsKey(Constants.HeaderToken) ?? false) && !string.IsNullOrEmpty(Request?.Headers[Constants.HeaderToken].ToString());

    private string CurrentUserId =>
        HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
        string.Empty;

    private string CurrentAuthToken =>
        Request.Headers[Constants.HeaderToken].ToString();
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiAuthorizationRequirement requirement)
    {
        if (requirement.AllowTokenAuthentication && await ValidateTokenAuthenticatedRequest() ||
            requirement.AllowUsersAuthentication && await ValidateUserAuthenticatedRequest())
        {
            context.Succeed(requirement);
            return;
        }
        context.Fail();
    }

    private async Task<bool> ValidateUserAuthenticatedRequest()
    {
        if (!IsUserAuthenticated || string.IsNullOrEmpty(CurrentUserId))
            return false;
        return await _authorization.ValidateUserAuthenticatedRequest(CurrentUserId, RequestController, RequestAction);
    }
    private async Task<bool> ValidateTokenAuthenticatedRequest()
    {
        if (!IsTokenAuthenticated || string.IsNullOrEmpty(CurrentAuthToken))
            return false;
        return await _authorization.ValidateTokenAuthenticatedRequest(CurrentAuthToken, RequestController, RequestAction);
    }
}