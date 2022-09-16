﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Components.Children;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.WasmClient.Client.Components.Interop;
using UntoMeWorld.WasmClient.Client.Components.ListControls;
using UntoMeWorld.WasmClient.Client.Data.Model;
using UntoMeWorld.WasmClient.Client.Utils.Extensions;

namespace UntoMeWorld.WasmClient.Client.Pages.Children;

[Authorize]
public class ChildrenPageBase : ComponentBase
{
    [Inject] public IChildrenService ChildrenService { get; set; } = null!;
    [Inject] public ToastService ToastService { get; set; } = null!;
    [CascadingParameter] public DialogsService? DialogService { get; set; }

    protected ChildrenList? ListView;
    protected ListController<string, Child>? ListController;



    protected override async Task OnInitializedAsync()
    {
        var paginationHelper = new PaginationHelper<Child>(r => ChildrenService.PaginateActive(r)) { PageSize = 200 };
        
        ListController = new ListController<string, Child>(c => c.Id, paginationHelper)
        {
            OnDataRefresh = async () =>
            {
                if (ListView != null)
                    await ListView.Reset();
            }
            
        };
        await ListController.SetFilter(null);
    }

    protected Task OnSortFieldChanged(SortField sortField)
    {
        return ListController?.SetSortField(sortField) ?? Task.CompletedTask;
    }

    protected Task OnSearchSubmit(string query)
    {
        return ListController?.SetTextQuery(query) ?? Task.CompletedTask;
    }

    protected void AddChild()
    {
        DialogService?.ShowDialog<ChildrenDialog, Child, Child>(null, async child =>
        {
            if (child != null)
            {
                try
                {
                    await ChildrenService.Add(child);
                    await ListController?.Refresh()!;
                    await ToastService.Success("Added",$"{child.Name} was successfully added!").ShowAsync();
                }
                catch (Exception e)
                {
                    await ToastService.Error(e.GetType().Name,e.Message).ShowAsync();
                }
            }
        }, isCancellable: false);
    }

    protected void EditChild(Child editChild)
    {
        DialogService?.ShowDialog<ChildrenDialog, Child, Child>(editChild, async child =>
        {
            if (child != null)
            {
                try
                {
                    await ChildrenService.Update(child);
                    await ListController?.Refresh()!;
                    await ToastService.Success("Edited",$"changes on {child.Name} were saved successfully!").ShowAsync();
                }
                catch (Exception e)
                {
                    await ToastService.Error(e.GetType().Name,e.Message).ShowAsync();
                }
            }
        }, isCancellable: false);
    }

    protected void DeleteChild(Child child)
    {
        DialogService?.ShowConfirmationAsync("Delete", "Are you sure you want to delete this?",
            async answer =>
            {
                if (!answer)
                    return;

                try
                {
                    await ChildrenService.Delete(child.Id);
                    await ListController?.Refresh()!;
                    await ToastService.Success("Deleted",$"{child.Name} was successfully deleted").ShowAsync();
                }
                catch (Exception e)
                {
                    await ToastService.Error(e.GetType().Name,e.Message).ShowAsync();
                }
            });
    }
}