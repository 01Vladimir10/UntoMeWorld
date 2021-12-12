namespace UntoMeWorld.WasmClient.Client.Models;

public class MenuItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsSelected { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public List<MenuItem> Items { get; set; } = new();
}