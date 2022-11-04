using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class DropDownOption<T>
{
    public T Value { get; set; } = default!;
    public string DisplayValue { get; set; } = string.Empty;
    public RenderFragment? Icon { get; set; }
    public DropDownOption()
    {
        
    }

    public DropDownOption(T value, string displayValue, RenderFragment icon = null)
    {
        Icon = icon;
        Value = value;
        DisplayValue = displayValue;
    }

    public DropDownOption(string displayValue, T value, RenderFragment icon = null)
    {
        Icon = icon;
        Value = value;
        DisplayValue = displayValue;
    }
}

public static class DropDownOptionExtensions
{
    public static IEnumerable<DropDownOption<T>> ToDropDownOptionsList<T>(this IEnumerable<T> source,
        Func<T, string> selector) =>
        source.Select(i => new DropDownOption<T>(selector(i), i));
    public static IEnumerable<DropDownOption<TValue>> ToDropDownOptionsList<T, TValue>(this IEnumerable<T> source,
        Func<T, string> displayNameSelector,
        Func<T, TValue> valueSelector) =>
        source.Select(i => new DropDownOption<TValue>(displayNameSelector(i), valueSelector(i)));
}