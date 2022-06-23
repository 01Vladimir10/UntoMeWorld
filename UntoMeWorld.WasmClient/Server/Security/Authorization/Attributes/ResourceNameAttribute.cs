

using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Common;

namespace UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;

public class ResourceNameAttribute : Attribute
{
    public ApiResource ApiResource { get; set; }

    public ResourceNameAttribute(ApiResource apiResource)
    {
        ApiResource = apiResource;
    }
}