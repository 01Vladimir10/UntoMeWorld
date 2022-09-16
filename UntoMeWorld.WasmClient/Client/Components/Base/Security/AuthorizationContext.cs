using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using UntoMeWorld.Domain.Security;

namespace UntoMeWorld.WasmClient.Client.Components.Base.Security;

/***
 * Cascades de ApiResource set through the ApiResource parameter to all its children
 */
public class AuthorizationContext : ComponentBase
{
    [Parameter] public ApiResource ApiResource { get;set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<ApiResource>>(0);
        builder.AddAttribute(1,"Value", ApiResource);
        builder.AddAttribute(2, "ChildContent", ChildContent);
        builder.CloseComponent();
        base.BuildRenderTree(builder);
    }
}