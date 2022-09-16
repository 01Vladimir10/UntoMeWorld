namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class ListItem<TKey, TItem>
{
    private bool _isSelected;
    public TKey? Key { get; set; }
    public TItem? Item { get; set; }
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value == _isSelected) return;
            _isSelected = value;
            OnSelectionChanged.Invoke(this, value);
        }
    }

    public Action<ListItem<TKey, TItem>, bool> OnSelectionChanged { get; set; } = (_, _) => { };
}
