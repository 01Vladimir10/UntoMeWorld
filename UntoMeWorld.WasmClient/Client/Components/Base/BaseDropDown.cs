using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class BaseDropDown<T> : ComponentBase, IComponent
{
    private T _selectedValue;
    private IEnumerable<DropDownOption<T>> _options;

    [Parameter]
    public IEnumerable<DropDownOption<T>> Options
    {
        get => _options;
        set
        {
            _options = value;
            SelectedOption = Options.FirstOrDefault(o => o.Value.Equals(SelectedValue)) ?? Options.FirstOrDefault();
            OptionsChanged.InvokeAsync();
        }
    }

    [Parameter] public T DefaultOption { get; set; }
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public Func<T, Task> OnSelectionChangedAsync { get; set; }
    [Parameter] public Action<T> OnSelectionChanged { get; set; }
    [Parameter] public DropDownOption<T> SelectedOption { get; set; }

    [Parameter]
    public T SelectedValue
    {
        get => _selectedValue;
        set
        {
            if (value == null || value.Equals(_selectedValue))
                return;
            _selectedValue = value;
            SelectedOption = Options.FirstOrDefault(o => o.Value.Equals(value));
            SelectedValueChanged.InvokeAsync(value);
        }
    }
    
    [Parameter]
    public EventCallback<T> SelectedValueChanged { get; set; }
    [Parameter]
    public EventCallback<IEnumerable<DropDownOption<T>>> OptionsChanged { get; set; }

    [Parameter] public int MaxDisplayCharLength { get; set; } = 100;
    
    public void AddSelectionChangedListener(Func<T, Task> onSelectionChanged)
    {
        if (OnSelectionChangedAsync == null)
            OnSelectionChangedAsync = onSelectionChanged;
        else
            OnSelectionChangedAsync += onSelectionChanged;
    }
}