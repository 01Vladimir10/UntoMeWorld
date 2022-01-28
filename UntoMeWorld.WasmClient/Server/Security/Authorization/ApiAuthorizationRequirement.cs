using Microsoft.AspNetCore.Authorization;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization;

public class ApiAuthorizationRequirement : IAuthorizationRequirement
{
    public bool AllowUsersAuthentication { get; set; } = true;
    public bool AllowTokenAuthentication { get; set; } = true;
}