using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public enum HtmlElement
{
    Div,
    Button,
    Span,
    I
}
public enum IconWeight
{
    Thin,
    Light,
    Regular,
    Bold,
    Fill
}
public abstract class Icon : ComponentBase
{
    [Parameter] public IconWeight Weight { get; set; } = IconWeight.Regular;
    [Parameter] public HtmlElement HtmlElement { get; set; } = HtmlElement.I;
    [Parameter] public string? CssClass { get; set; }
    [Parameter] public object? IconName { get; set; }
}