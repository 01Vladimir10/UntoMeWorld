using Microsoft.AspNetCore.Components;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Components.Children;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.WasmClient.Client.Components.Icons;
using UntoMeWorld.WasmClient.Client.Components.Interop;
using UntoMeWorld.WasmClient.Client.Components.ListControls;
using UntoMeWorld.WasmClient.Client.Data.Model;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.Pages.Children;

public class ChildrenPageBase : ComponentBase
{
    [Inject] public IChildrenService ChildrenService { get; set; }
    [Inject] public ToastService ToastService { get; set; }

    [CascadingParameter] public DialogsService DialogService { get; set; }

    protected ChildrenList ListView;
    protected ListController<string, Child> ListController;



    protected override async Task OnInitializedAsync()
    {
        var paginationHelper = new PaginationHelper<Child>(ChildrenService.Paginate) { PageSize = 50 };
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
        return ListController.SetSortField(sortField);
    }

    protected Task OnSearchSubmit(string query)
    {
        return ListController.SetFilter(
            string.IsNullOrEmpty(query)
                ? null
                : QueryLanguage.TextSearch(query));
    }

    protected void AddChild()
    {
        DialogService.ShowDialog<ChildrenDialog, Child, Child>(null, async child =>
        {
            if (child != null)
            {
                try
                {
                    await ChildrenService.Add(child);
                    await ListController.Refresh();
                    await ToastService.ShowSuccessAsync("Saved successfully!", PhosphorIcons.Check);
                }
                catch (Exception e)
                {
                    await ToastService.ShowErrorAsync($"Error: {e.Message}", PhosphorIcons.XCircle);
                }
            }
        }, isCancellable: false);
    }

    protected void EditChild(Child editChild)
    {
        DialogService.ShowDialog<ChildrenDialog, Child, Child>(editChild, async child =>
        {
            if (child != null)
            {
                try
                {
                    await ChildrenService.Update(child);
                    await ListController.Refresh();
                    await ToastService.ShowSuccessAsync("Saved successfully!", PhosphorIcons.Check);
                }
                catch (Exception e)
                {
                    await ToastService.ShowErrorAsync($"Error: {e.Message}", PhosphorIcons.XCircle);
                }
            }
        }, isCancellable: false);
    }

    protected void DeleteChild(Child child)
    {
        DialogService.ShowConfirmationAsync("Delete", "Are you sure you want to delete this?",
            async answer =>
            {
                if (!answer)
                    return;

                try
                {
                    await ChildrenService.Delete(child.Id);
                    await ListController.Refresh();
                    await ToastService.ShowSuccessAsync("Deleted successfully!", PhosphorIcons.Check);
                }
                catch (Exception e)
                {
                    await ToastService.ShowErrorAsync($"Error: {e.Message}", PhosphorIcons.XCircle);
                }
            });
    }
}