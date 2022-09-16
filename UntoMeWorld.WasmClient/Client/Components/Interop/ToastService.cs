using Microsoft.JSInterop;
using Newtonsoft.Json;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Utils.Common;

namespace UntoMeWorld.WasmClient.Client.Components.Interop;

public enum ToastStyle
{
    Info,
    Success,
    Warning,
    Error
}

public enum ToastDuration
{
    Long = 10000,
    Medium = 6000,
    Short = 3000
}

public class Toast
{
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string CssClass { get; set; } = "";
    public ToastStyle ToastStyle { get; set; }
    private readonly ToastService _service;

    public Toast(ToastService service)
    {
        _service = service;
    }

    public void Show(ToastDuration duration = ToastDuration.Short)
    {
        _service.Show(this, duration);
    }

    public Task ShowAsync(ToastDuration duration = ToastDuration.Short)
    {
        return _service.ShowAsync(this, duration);
    }
}

public class ToastService
{
    private readonly IJSRuntime _runtime;
    private readonly ThrottleDispatcher _dispatcher;

    public ToastService(IJSRuntime runtime)
    {
        _runtime = runtime;
        _dispatcher = new ThrottleDispatcher(TimeSpan.FromMilliseconds(250));
    }

    public Toast Error(string title, string subtitle = "")
        => Create(title, subtitle, ToastStyle.Error);

    public Toast Success(string title, string subtitle = "")
        => Create(title, subtitle, ToastStyle.Success);
    public Toast Warning(string title, string subtitle = "")
        => Create(title, subtitle, ToastStyle.Warning);
    public Toast Info(string title, string subtitle = "")
        => Create(title, subtitle);

    public Toast Create(string title, string subTitle = "", ToastStyle style = ToastStyle.Info, string cssClass = "")
        => new(this)
        {
            Title = title,
            SubTitle = subTitle,
            ToastStyle = style,
            CssClass = cssClass
        };

    private Task ShowToast(Toast toast, ToastDuration duration)
    {
        _dispatcher.AddTask(async () =>
        {
            Console.WriteLine("Showing toast " + JsonConvert.SerializeObject(toast));
            await _runtime.InvokeVoidAsync(InteropFunctions.CreateToast, toast.ToastStyle.ToString().ToLower(),
                toast.Title, toast.SubTitle, toast.CssClass, (int)duration);
        });
        return Task.CompletedTask;
    }

    public async void Show(Toast toast, ToastDuration duration) => await ShowToast(toast, duration);
    public async Task ShowAsync(Toast toast, ToastDuration duration) => await ShowToast(toast, duration);
}