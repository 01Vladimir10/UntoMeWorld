using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.WasmClient.Client.Data.Model;
using UntoMeWorld.WasmClient.Client.Services.Base;
using UntoMeWorld.WasmClient.Client.Utils.UIHelpers;

namespace UntoMeWorld.WasmClient.Client.ViewModels;
public class GenericViewModel<TModel> : BaseViewModel where TModel : IModel
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
                throw new Exception($"The item of type {typeof(TModel).Name} could not be updated");
            _itemsDictionary.Add(newItem.Id, newItem);
            OnPropertyChanged(nameof(Items));
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
            OnPropertyChanged(nameof(Items));
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
            OnPropertyChanged(nameof(Items));
        }
        catch (Exception e)
        {
            OnError(e);
        }
    }
    public async Task Filter(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            await UpdateList();
        Items = (await _service.Paginate(QueryLanguage.TextSearch(query))).Result;
    }
    public async Task UpdateList()
    {
        Items =  (await _service.Paginate(null, SortField.FieldName, SortField.Descendent)).Result;
    }
    public async Task SortElementsBy(string fieldName, bool desc = false)
    {
        SortField = new SortField
        {
            FieldName = fieldName,
            Descendent = desc
        };
        await UpdateList();
        OnPropertyChanged(nameof(SortField));
    }
    #region Properties
    public IEnumerable<TModel> Items
    {
        get => _itemsDictionary.Values;
        set
        {
            _itemsDictionary = value.ToDictionary(i => i.Id, i => i);
            OnPropertyChanged(nameof(Items));
        }
    }
    
    public Action<Exception> OnError { get; set; } = _ => { };
    
    #endregion

    #region MultiSelection
    public MultiSelectController<Church> MultiSelectController { get; } = new();
    public bool IsMultiSelecting
    {
        get => MultiSelectController.IsMultiSelecting;
        set
        {
            MultiSelectController.IsMultiSelecting = value;
            OnPropertyChanged(nameof(IsMultiSelecting));
        }
    }
    private void InitMultiSelection()
    {
        MultiSelectController.OnMultiSelectStop = () =>
        {
            IsMultiSelecting = false;
            OnPropertyChanged(nameof(IsMultiSelecting));
        };
    }

    #endregion
}