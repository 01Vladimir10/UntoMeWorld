using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseHtmlComponent : ComponentBase
{
    [Parameter]
    public string? CssClass { get; set; }
    
    [Parameter] public Func<Task>? OnClickAsync { get; set; }
    [Parameter] public Action? OnClick { get; set; }

    [Parameter] public bool IsVisible { get; set; } = true;
    
    [Parameter]
    public string? Id { get; set; }
}