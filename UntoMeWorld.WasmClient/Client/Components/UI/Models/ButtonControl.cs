using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.UI.Models;

public abstract class ButtonControl : BaseControl
{
    [Parameter]
    public RenderFragment Content { get; set; }
    
    [Parameter] public Func<Task> OnClick { get; set; }
}