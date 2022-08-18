using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.Children;

public class BaseChildrenDialog : BaseDialog<Child, bool>
{
    protected bool IsNewChild { get; set; } = true;
    protected Child Child = new();
    protected List<DropDownOption<Church>> ChurchesOptions = new() {new DropDownOption<Church>("", new Church())};
    
    [Inject] public IChurchesStore ChurchesStore { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsNewChild = Parameter == null;
            Child = Parameter == null
                ? new Child()
                : Parameter.Clone();
            var churches = await ChurchesStore.All();
            ChurchesOptions = churches.ToDropDownOptionsList(c => c.Name).ToList();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected Task OnChurchSelectionChanged(Church church)
    {
        Child.ChurchId = church.Id;
        Child.Church = church;
        return  Task.CompletedTask;
    }

    protected async Task Close()
    {
        Console.WriteLine(JsonConvert.SerializeObject(Child));
        await OnCloseAsync(false);
    }
}