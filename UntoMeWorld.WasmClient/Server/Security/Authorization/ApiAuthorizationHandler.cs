using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization;

public class ApiAuthorizationHandler : AuthorizationHandler<ApiAuthorizationRequirement>
{
    private readonly IApiAuthorizationService _authorization;
    private readonly IHttpContextAccessor _contextAccessor;

    private HttpContext HttpContext => _contextAccessor.HttpContext;

    public ApiAuthorizationHandler(IApiAuthorizationService authorization, IHttpContextAccessor contextAccessor)
    {
        _authorization = authorization;
        _contextAccessor = contextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiAuthorizationRequirement requirement)
    {
        var isHeaderAuthentication = (HttpContext?.Request.Headers.ContainsKey(Constants.HeaderToken) ?? false) && !string.IsNullOrEmpty(HttpContext.Request.Headers[Constants.HeaderToken].ToString());
        var isUserAuthentication = context.User.Identity?.IsAuthenticated ?? false;

        if (!isHeaderAuthentication && !isUserAuthentication)
        {
            context.Fail(new AuthorizationFailureReason(this, "User not authenticated and token is not present"));
            return;
        }

        var controller = HttpContext?.Request.RouteValues["controller"]?.ToString() ?? "";
        var action = HttpContext?.Request.RouteValues["action"]?.ToString() ?? "";

        if (isUserAuthentication)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail(new AuthorizationFailureReason(this, "User authentication failed, the user does not have a valid claim with its id"));
                return;
            }
            if (!await _authorization.ValidateUserAuthenticatedRequest(userId, controller, action))
            {
                context.Fail(new AuthorizationFailureReason(this, "User does not have permissions to access this resource"));
                return;
            }
            context.Succeed(requirement);
            return;
        }

        var token = HttpContext?.Request.Headers[Constants.HeaderToken].ToString();
        if (!await _authorization.ValidateTokenAuthenticatedRequest(token, controller, action))
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not have permissions to access this resource"));
            return;
        }
        context.Succeed(requirement);
    }
}