using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.WasmClient.Client.Data.Model;
using UntoMeWorld.WasmClient.Client.Utils.UIHelpers;

namespace UntoMeWorld.WasmClient.Client.ViewModels;
public class GenericViewModel<TModel> where TModel : IModel
{
    private readonly IService<TModel> _service;
    private IDictionary<string, TModel> _itemsDictionary = new Dictionary<string, TModel>();
    public SortField SortField { get; private set; } = new();

    protected GenericViewModel(IService<TModel> service)
    {
        _service = service;
        InitMultiSelection();
    }


    public async Task Add(TModel item)
    {
        try
        {
            var newItem = await _service.Add(item);
            if (newItem == null || string.IsNullOrEmpty(newItem.Id))
                throw new Exception($"The item of type {typeof(TModel).Name} could not be added");
            _itemsDictionary.Add(newItem.Id, newItem);
        }
        catch (Exception e)
        {
            OnError(e);
        }
    }
    public async Task Update(TModel item)
    {
        try
        {
            item = await _service.Update(item);
            if (item == null)
                throw new Exception($"The item of type {typeof(TModel).Name} could not be updated");
            
            _itemsDictionary[item.Id] = item;
        }
        catch (Exception e)
        {
            OnError(e);
        }
    }
    public async Task Delete(TModel item)
    {
        try
        {
            await _service.Delete(item.Id);
            if(!_itemsDictionary.Remove(item.Id))
                throw new Exception($"The item of type {typeof(TModel).Name} could not be deleted");
        }
        catch (Exception e)
        {
            OnError(e);
        }
    }
    public async Task UpdateList()
    {
        var result = await _service.Query(null, null, SortField.FieldName, SortField.Descendent);
        Items =  result.Result ?? new List<TModel>();
    }
    public async Task SortElementsBy(string fieldName, bool desc = false)
    {
        SortField = new SortField
        {
            FieldName = fieldName,
            Descendent = desc
        };
        await UpdateList();
    }
    #region Properties
    public IEnumerable<TModel> Items
    {
        get => _itemsDictionary.Values;
        set
        {
            _itemsDictionary = value.ToDictionary(i => i.Id, i => i);
        }
    }
    
    public Action<Exception> OnError { get; set; } = _ => { };
    
    #endregion

    #region MultiSelection
    public MultiSelectController<Church> MultiSelectController { get; } = new();
    public bool IsMultiSelecting
    {
        get => MultiSelectController.IsMultiSelecting;
        set => MultiSelectController.IsMultiSelecting = value;
    }
    private void InitMultiSelection()
    {
        MultiSelectController.OnMultiSelectStop = () =>
        {
            IsMultiSelecting = false; 
        };
    }
    #endregion
}