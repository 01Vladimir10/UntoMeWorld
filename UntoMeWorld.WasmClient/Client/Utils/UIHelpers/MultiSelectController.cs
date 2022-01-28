using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.WasmClient.Client.Utils.UIHelpers;

public class MultiSelectController<TModel> where TModel : IModel
{
    public bool IsMultiSelecting { get; set; }
    public Action OnMultiSelectStop { get; set; }
    
    public int SelectedItemsCount => SelectedItemsKeys.Count;
    protected HashSet<string> SelectedItemsKeys { get; set; } = new();

    public MultiSelectController()
    {
        
    }
    
    public bool IsItemSelected(TModel item)
        => SelectedItemsKeys.Contains(item.Id);
    
    public void ToggleItem(TModel item)
    {
        if (SelectedItemsKeys.Contains(item.Id))
            SelectedItemsKeys.Remove(item.Id);
        else
            SelectedItemsKeys.Add(item.Id);

        if (SelectedItemsKeys.Any()) return;
        IsMultiSelecting = false;
        OnMultiSelectStop();
    }

    public void StopMultiSelection()
    {
        IsMultiSelecting = false;
        SelectedItemsKeys.Clear();
        OnMultiSelectStop();
    }

    protected void InitMultiSelection()
    {
        SelectedItemsKeys = new HashSet<string>();
    }
}