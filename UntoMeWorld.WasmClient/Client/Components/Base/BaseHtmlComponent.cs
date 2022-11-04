using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseHtmlComponent : ComponentBase
{
    [Parameter]
    public string? CssClass { get; set; }
    

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; } = EventCallback<MouseEventArgs>.Empty;

    [Parameter] public bool IsVisible { get; set; } = true;
    
    [Parameter]
    public string? Id { get; set; }
}