

using UntoMeWorld.WasmClient.Server.Common;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;

public class ResourceNameAttribute : Attribute
{
    public ResourceType Resource { get; set; }

    public ResourceNameAttribute(ResourceType resource)
    {
        Resource = resource;
    }
}