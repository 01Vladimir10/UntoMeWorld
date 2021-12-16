namespace UntoMeWorld.WasmClient.Client.Components.UI.Models;

public class SelectOption<T>
{
    public T Value { get; set; }
    public string DisplayValue { get; set; }
    
    public bool Selected { get; set; }
}

public static class SelectOptionExtensions
{
    public static IEnumerable<SelectOption<TOption>> ToSelectOptions<T, TOption>(this IEnumerable<T> array,
        Func<T, TOption> valueSelector, Func<T, string> displayNameSelector = null)
        => array.Select(x => new SelectOption<TOption>
        {
            Value = valueSelector(x),
            DisplayValue = displayNameSelector?.Invoke(x)
        });
}