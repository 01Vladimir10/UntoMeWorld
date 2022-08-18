using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class BaseDropDown<T> : ComponentBase, IComponent
{
    [Parameter] public IEnumerable<DropDownOption<T>> Options { get; set; }
    [Parameter] public T DefaultOption { get; set; }
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public Func<T, Task> OnSelectionChangedAsync { get; set; }
    [Parameter] public Action<T> OnSelectionChanged { get; set; }
    [Parameter] public DropDownOption<T> SelectedOption { get; set; }
    [Parameter] public T SelectedValue { get; set; }
    [Parameter] public int MaxDisplayCharLength { get; set; } = 100;
    
    public void AddSelectionChangedListener(Func<T, Task> onSelectionChanged)
    {
        if (OnSelectionChangedAsync == null)
            OnSelectionChangedAsync = onSelectionChanged;
        else
            OnSelectionChangedAsync += onSelectionChanged;
    }
}