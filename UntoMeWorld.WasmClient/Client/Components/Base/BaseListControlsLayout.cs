using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseListControlsLayout : ComponentBase, IComponent
{
    [Parameter] public string CssClass { get; set; }
    [Parameter] public bool IsMultiSelecting { get; set; }
    [Parameter] public RenderFragment NormalControls { get; set; }
    [Parameter] public RenderFragment MultiSelectControls { get; set; }
}