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

        SelectedOption = Options.First();
        if (DefaultOption != null)
            SelectedOption = Options.FirstOrDefault(o => o.Value.Equals(DefaultOption));
        _isFirstTime = true;
    }

    protected void OpenDropdown()
    {
        IsOpened = !IsOpened;
    }

    protected Task OnOptionSelected(DropDownOption<T> option)
    {
        SelectedOption = option;
        return OnSelectionChanged == null ? Task.CompletedTask : OnSelectionChanged.Invoke(option.Value);
    }
}