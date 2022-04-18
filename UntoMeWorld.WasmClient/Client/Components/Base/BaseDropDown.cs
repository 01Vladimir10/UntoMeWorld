using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class BaseDropDown<T> : ComponentBase, IComponent
{
    [Parameter] public IEnumerable<DropDownOption<T>> Options { get; set; }
    [Parameter] public T DefaultOption { get; set; }
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public Func<T, Task> OnSelectionChanged { get; set; }
    public DropDownOption<T> SelectedOption { get; set; }
    
    public void AddSelectionChangedListener(Func<T, Task> onSelectionChanged)
    {
        if (OnSelectionChanged == null)
            OnSelectionChanged = onSelectionChanged;
        else
            OnSelectionChanged += onSelectionChanged;
    }
}