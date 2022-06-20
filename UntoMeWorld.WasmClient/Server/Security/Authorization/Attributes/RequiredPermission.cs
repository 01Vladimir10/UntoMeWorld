using UntoMeWorld.Domain.Security;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;

public class RequiredPermissionAttribute : Attribute
{
    public PermissionType RequiredPermission { get; }
    
    public RequiredPermissionAttribute(PermissionType requiredPermission)
    {
        RequiredPermission = requiredPermission;
    }
}