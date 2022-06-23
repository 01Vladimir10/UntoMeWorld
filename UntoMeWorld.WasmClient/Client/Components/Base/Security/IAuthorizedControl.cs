using Microsoft.AspNetCore.Components;
using UntoMeWorld.Domain.Security;

namespace UntoMeWorld.WasmClient.Client.Components.Base.Security;

public interface IAuthorizedControl
{
    [Parameter]
    public ApiResource ApiResource { get; set; }
    
    [Parameter]
    public PermissionType RequiredPermission { get; set; }
}