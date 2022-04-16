using Microsoft.AspNetCore.Components;
using UntoMeWorld.WasmClient.Client.Components.Icons;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class SortByOption<T>
{
    public T Value { get; set; }
    public string DisplayValue { get; set; }
    #nullable enable
    public RenderFragment? Icon { get; set; }
    #nullable disable
    public SortByOption()
    {
        
    }

    public SortByOption(T value, string displayValue, RenderFragment icon = null)
    {
        Icon = icon;
        Value = value;
        DisplayValue = displayValue;
    }
}

public class BaseDropDown<T> : ComponentBase, IComponent
{
    [Parameter] public IEnumerable<SortByOption<T>> Options { get; set; }
    [Parameter] public Func<T, Task> OnSelectionChanged { get; set; }
    [Parameter] public T DefaultOption { get; set; }
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public string CssClass { get; set; }
    public SortByOption<T> SelectedOption { get; set; }
}