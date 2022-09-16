using Microsoft.AspNetCore.Components;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Validation;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Components.Interop;

namespace UntoMeWorld.WasmClient.Client.Components.Children;

public class BaseChildrenDialog : BaseDialog<Child, Child>
{
    protected bool IsNewChild { get; set; } = true;
    protected Child Child = new();
    protected IEnumerable<DropDownOption<Church>> ChurchesOptions = new List<DropDownOption<Church>>() { new("", new Church()) };

    [Inject] public IChurchesService ChurchesService { get; set; } = null!;
    [Inject] public ToastService ToastService { get; set; } = null!;
    private bool _isFirstRender = true;

    private static List<Church>? _churches;

    protected override async Task OnParametersSetAsync()
    {
        if (_isFirstRender)
        {
            IsNewChild = Parameter == null;
            Child = Parameter == null
                ? new Child()
                : Parameter.Clone();
            _churches ??= await ChurchesService.GetAll().ContinueWith(t => t.Result.ToList());
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
                ToastService.Error("Validation error",  validationResult.ToString()).Show(ToastDuration.Medium);
            }

            return;
        }

        Child.BraSize = Child.Gender == Gender.Female ? Child.BraSize : UnderwearSize.NotApplicable;
        Child.ChurchId = Child.Church?.Id;
        await OnCloseAsync(Child);
    }
}