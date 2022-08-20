using Newtonsoft.Json;
using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.ListControls;

public class DropDownBase<T> : BaseDropDown<T>
{
    protected bool IsOpened { get; private set; }

    private bool _isFirstTime = true;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!_isFirstTime)
            return;
        
        if (SelectedOption == null && DefaultOption != null)
            SelectedOption = Options.FirstOrDefault(o => o.Value.Equals(DefaultOption)) ?? Options.FirstOrDefault();
        
        _isFirstTime = false;
    }

    protected void OpenDropdown()
    {
        IsOpened = !IsOpened;
    }

    protected async Task OnOptionSelected(DropDownOption<T> option)
    {
        await InvokeAsync(() =>
        {
            SelectedOption = option;
            SelectedValue = option.Value;
            OnSelectionChanged?.Invoke(option.Value);
            OnSelectionChangedAsync?.Invoke(option.Value);
        });
    }
}

public static class DropDowns
{
    public static readonly IReadOnlyList<DropDownOption<bool>> Boolean = new List<DropDownOption<bool>>
    {
        new(true, "Yes"), 
        new(false, "No")
    };

    public static List<DropDownOption<T>> FromEnum<T>() where T :  struct, Enum
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("T is not an Enum type");
        return Enum.GetValues<T>().Select(e => new DropDownOption<T>(e, Enum.GetName(e))).ToList();
    }
}