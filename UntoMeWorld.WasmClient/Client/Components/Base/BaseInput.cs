using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class BaseInput<T> : ComponentBase
{
    protected virtual T? CurrentValue { get; set; }

    [Parameter]
    public virtual T? Value
    {
        get => CurrentValue;
        set
        {
            if (AreEqual(CurrentValue, value))
            {
                return;
            }

            UpdateValueAsync(value);
        }
    }

    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    protected virtual bool AreEqual(T? currentValue, T? newValue) =>
        EqualityComparer<T>.Default.Equals(currentValue, newValue);

    protected virtual Task UpdateValueAsync(T? value)
    {
        CurrentValue = value;
        return ValueChanged.InvokeAsync(value);
    }
}