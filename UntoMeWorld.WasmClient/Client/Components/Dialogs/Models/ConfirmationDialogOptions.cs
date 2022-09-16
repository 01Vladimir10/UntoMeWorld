namespace UntoMeWorld.WasmClient.Client.Components.Dialogs.Models;

public class ConfirmationDialogOptions
{
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string? OkText { get; set; }
    public string? CancelText { get; set; }
}