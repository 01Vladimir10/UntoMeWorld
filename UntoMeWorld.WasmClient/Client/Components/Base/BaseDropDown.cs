using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class BaseDropDown<T> : BaseInput<T>, IComponent
{
    private IEnumerable<DropDownOption<T>> _options = Enumerable.Empty<DropDownOption<T>>();

    [Parameter]
    public IEnumerable<DropDownOption<T>> Options
    {
        get => _options;
        set
        {
            _options = value;
            OptionsChanged.InvokeAsync();
        }
    }

    [Parameter] public T? DefaultOption { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public string? CssClass { get; set; }
    

    [Parameter] public EventCallback<T> SelectedValueChanged { get; set; }
    [Parameter] public EventCallback<IEnumerable<DropDownOption<T>>> OptionsChanged { get; set; }

    [Parameter] public int MaxDisplayCharLength { get; set; } = 100;
}