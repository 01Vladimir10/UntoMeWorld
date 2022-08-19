using Microsoft.AspNetCore.Components;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.Children;

public class BaseChildrenDialog : BaseDialog<Child, Child>
{
    protected bool IsNewChild { get; set; } = true;
    protected Child Child = new();
    protected List<DropDownOption<Church>> ChurchesOptions = new() { new DropDownOption<Church>("", new Church()) };

    [Inject] public IChurchesStore ChurchesStore { get; set; }
    private bool _isFirstRender = true;

    protected override async Task OnParametersSetAsync()
    {
        if (_isFirstRender)
        {
            IsNewChild = Parameter == null;
            Child = Parameter == null
                ? new Child()
                : Parameter.Clone();
            var churches = await ChurchesStore.All();
            ChurchesOptions = churches.ToDropDownOptionsList(c => c.Name).ToList();
        }
        _isFirstRender = false;
        await base.OnParametersSetAsync();
    }

    protected async Task Close()
    {
        await OnCloseAsync(null);
    }

    protected async Task Save()
    {
        var result = ModelValidator.Validate(Child);
        if (result.IsValid)
        {
            Child.ChurchId = Child.Church?.Id;
            await OnCloseAsync(Child);
            return;
        }

        Console.WriteLine(string.Join(",", result.Results));
    }
}