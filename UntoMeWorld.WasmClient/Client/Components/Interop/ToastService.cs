﻿using Microsoft.JSInterop;
using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.Interop;

public enum ToastStyle
{
    General,
    Primary,
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

    public string Content { get; set; }
    public string Icon { get; set; }
    public string CssClass { get; set; }
}

public class ToastService
{
    private readonly IJSRuntime _runtime;

    public ToastService(IJSRuntime runtime)
    {
        _runtime = runtime;
    }

    public Task ShowErrorAsync(string content, string icon = null, ToastDuration duration = ToastDuration.Medium)
    {
        return Create(content, icon, ToastStyle.Error).ShowAsync(duration);
    } 
    public Task ShowSuccessAsync(string content, string icon = null, ToastDuration duration = ToastDuration.Medium)
    {
        return Create(content, icon, ToastStyle.Success).ShowAsync(duration);
    } 

    public Toast Create(string content, string icon = null, ToastStyle style = ToastStyle.General,
        IconWeight iconWeight = IconWeight.Bold, string cssClass = null)
        => new Toast(this)
        {
            Content = content,
            Icon = string.IsNullOrEmpty(icon) ? "" : $"{icon}-{iconWeight.ToString().ToLower()}",
            CssClass = $"{style.ToString().ToLower()} {cssClass}"
        };


    public async void Show(Toast toast, ToastDuration duration)
    {
        await _runtime.InvokeVoidAsync(InteropFunctions.CreateToast, toast.Content, toast.Icon, toast.CssClass,
            (int)duration);
    }
    
    public async Task ShowAsync(Toast toast, ToastDuration duration)
    {
        await _runtime.InvokeVoidAsync(InteropFunctions.CreateToast, toast.Content, toast.Icon, toast.CssClass,
            (int)duration);
    }
}