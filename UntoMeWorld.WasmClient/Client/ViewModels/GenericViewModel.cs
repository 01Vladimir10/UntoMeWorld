using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Data.Repositories;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public abstract class GenericViewModel<TModel> : BaseViewModel where TModel : IModel
{
    private readonly IRepository<TModel> _repository;
    private List<TModel> _items = new();
    public Action<Exception> OnError { get; set; } = _ => { };

    public GenericViewModel(IRepository<TModel> repository)
    {
        _repository = repository;
    }
    
    public async Task Add(TModel church)
    {
        try
        {
            var newChurch = await _repository.Add(church);
            if (newChurch == null)
                throw new Exception("The church could not be added");
            Items.Add(church);
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
            Console.WriteLine("Updating church = " + item!);
            var newChurch = await _repository.Update(item);
            if (newChurch == null)
                throw new Exception("The church could not be added");
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
            await _repository.Delete(item);
            var index = Items.FindIndex(i => i.Id == item.Id);
            if (index < 0)
                return;
            Items.RemoveAt(index);
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
        var result = await _repository.All(query);
        Items = result.ToList();
    }
    public async Task UpdateList()
    {
        var result = await  _repository.All();
        Items = result.ToList();
    }
    #region Properties
    public List<TModel> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }
    #endregion

}