using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Dialogs;

public class BaseDialog<TParam, TResult> : ComponentBase
{
    [Parameter]
    public TParam Parameter { get; set; }
    
    [Parameter]
    public Func<TResult, Task> OnCloseAsync { get; set; }
}