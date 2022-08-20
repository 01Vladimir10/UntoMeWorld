using UntoMeWorld.WasmClient.Client.Components.Dialogs.Models;

namespace UntoMeWorld.WasmClient.Client.Components.Dialogs;

public static class DialogExtensions
{
    public static void ShowConfirmationAsync(this DialogsService service, string title, string message, Func<bool,Task> onClose,
        string okText = "Ok", string cancelText = "Cancel", bool isCancellable = true)
    {
        var options = new ConfirmationDialogOptions
        {
            Title = title,
            SubTitle = message,
            OkText = okText,
            CancelText = cancelText
        };
        service.ShowDialog<ConfirmationDialog, ConfirmationDialogOptions, bool>(options, onClose, isCancellable);
    }
    
}