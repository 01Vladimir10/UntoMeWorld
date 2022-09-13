using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using UntoMeWorld.Application.Extensions.Security;
using UntoMeWorld.Application.Helpers;
using UntoMeWorld.Application.Services;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Shared.Security.Utils;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization;

public class ApiAuthorizationHandler : AuthorizationHandler<ApiAuthorizationRequirement>
{
    private readonly IApiAuthorizationService _authorization;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly CacheHelper<(ApiResource, PermissionType), string> _actionsCache;

    public ApiAuthorizationHandler(IApiAuthorizationService authorization, IHttpContextAccessor contextAccessor,
        IMemoryCache cache)
    {
        _authorization = authorization;
        _contextAccessor = contextAccessor;
        _actionsCache =
            new CacheHelper<(ApiResource, PermissionType), string>(cache, "ApiEndpointAttributes__",
                TimeSpan.MaxValue);
    }

    private HttpContext HttpContext => _contextAccessor.HttpContext;
    private HttpRequest Request => HttpContext?.Request;
    private bool IsUserAuthenticated => HttpContext?.User.Identity?.IsAuthenticated ?? false;


    private string CurrentRequestController => Request?.RouteValues["controller"]?.ToString() ?? string.Empty;
    private string CurrentRequestAction => Request?.RouteValues["action"]?.ToString() ?? string.Empty;

    private bool IsTokenAuthenticated => (Request?.Headers.ContainsKey(ServerConstants.HeaderToken) ?? false) &&
                                         !string.IsNullOrEmpty(Request?.Headers[ServerConstants.HeaderToken]
                                             .ToString());

    private AppUser CurrentUser => HttpContext?.User.Claims.ToAppUser();

    private string CurrentAuthToken =>
        Request.Headers[ServerConstants.HeaderToken].ToString();

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ApiAuthorizationRequirement requirement)
    {
        var (resource, permissionType) = await GetExecutingEndpointMetadata();
        if (resource == ApiResource.Unknown || permissionType == PermissionType.Unknown)
        {
            return;
        }

        if (requirement.AllowTokenAuthentication && await ValidateTokenAuthenticatedRequest(resource, permissionType) ||
            requirement.AllowUsersAuthentication && await ValidateUserAuthenticatedRequest(resource, permissionType))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }

    private Task<(ApiResource, PermissionType)> GetExecutingEndpointMetadata()
        => _actionsCache.Get($"{CurrentRequestController}_{CurrentRequestAction}", () =>
        {
            var metadata = HttpContext.GetEndpoint()?.Metadata ?? new EndpointMetadataCollection();
            var resourceAttr = metadata.OfType<ResourceNameAttribute>().FirstOrDefault();
            var permissionAttr = metadata.OfType<RequiredPermissionAttribute>().FirstOrDefault();
            var resource = resourceAttr?.ApiResource ?? ApiResource.Unknown;
            var permission = permissionAttr?.RequiredPermission ?? PermissionType.Unknown;
            return Task.FromResult((resource, permission));
        });

    private async Task<bool> ValidateUserAuthenticatedRequest(ApiResource apiResource, PermissionType permission)
    {
        if (!IsUserAuthenticated || CurrentUser == null)
            return false;
        return await _authorization.ValidateUserAuthenticatedRequest(CurrentUser, apiResource, permission);
    }

    private async Task<bool> ValidateTokenAuthenticatedRequest(ApiResource apiResource, PermissionType permission)
    {
        if (!IsTokenAuthenticated || string.IsNullOrEmpty(CurrentAuthToken))
            return false;
        return await _authorization.ValidateTokenAuthenticatedRequest(CurrentAuthToken, apiResource, permission);
    }
}