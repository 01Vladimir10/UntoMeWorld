using Microsoft.AspNetCore.Components;
using UntoMeWorld.WasmClient.Client.Components.Icons;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class DropDownOption<T>
{
    public T Value { get; set; }
    public string DisplayValue { get; set; }
    #nullable enable
    public RenderFragment? Icon { get; set; }
    #nullable disable
    public DropDownOption()
    {
        
    }

    public DropDownOption(T value, string displayValue, RenderFragment icon = null)
    {
        Icon = icon;
        Value = value;
        DisplayValue = displayValue;
    }
}