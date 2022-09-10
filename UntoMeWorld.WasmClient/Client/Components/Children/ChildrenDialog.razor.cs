using Microsoft.AspNetCore.Components;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Validation;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Components.Interop;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.Components.Children;

public class BaseChildrenDialog : BaseDialog<Child, Child>
{
    protected bool IsNewChild { get; set; } = true;
    protected Child Child = new();
    protected IEnumerable<DropDownOption<Church>> ChurchesOptions = new List<DropDownOption<Church>>() { new("", new Church()) };

    [Inject] public IChurchesService ChurchesService { get; set; }
    [Inject] public ToastService ToastService { get; set; }
    private bool _isFirstRender = true;

    private static List<Church> _churches = null;

    protected override async Task OnParametersSetAsync()
    {
        if (_isFirstRender)
        {
            IsNewChild = Parameter == null;
            Child = Parameter == null
                ? new Child()
                : Parameter.Clone();
            _churches ??= await ChurchesService.All();
            Child.Church ??= _churches.FirstOrDefault();
            ChurchesOptions = _churches.ToDropDownOptionsList(c => c.Name);
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
        if (!result.IsValid)
        {
            foreach (var validationResult in result.Results)
            {
                await ToastService
                    .Create("Error: " + validationResult, style: ToastStyle.Error)
                    .ShowAsync(ToastDuration.Medium);
            }

            return;
        }

        Child.BraSize = Child.Gender == Gender.Female ? Child.BraSize : UnderwearSize.NotApplicable;
        Child.ChurchId = Child.Church?.Id;
        await OnCloseAsync(Child);
    }
}