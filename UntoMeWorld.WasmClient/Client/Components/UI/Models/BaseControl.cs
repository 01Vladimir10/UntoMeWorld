using System.Text;
using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.UI.Models;

public abstract class BaseControl
{
    
    [Parameter] public string Class { get; set; }
    [Parameter] public bool IsEnabled { get; set; }
    [Parameter] public string MarginLeft { get; set; }
    [Parameter] public string MarginRight { get; set; }
    [Parameter] public string MarginTop { get; set; }
    [Parameter] public string MarginBottom { get; set; }
    [Parameter] public string Margin { get; set; }
    [Parameter] public string Padding { get; set; }
    [Parameter] public string PaddingTop { get; set; }
    [Parameter] public string PaddingLeft { get; set; }
    [Parameter] public string PaddingRight { get; set; }
    [Parameter] public string PaddingBottom { get; set; }
    [Parameter] public string FontSize { get; set; }
    [Parameter] public string FontWeight { get; set; }
    [Parameter] public string TextColor { get; set; }
    [Parameter] public string BackgroundColor { get; set; }

    protected string Styles()
    {
        var styles = new StringBuilder();
        var padding = string.IsNullOrEmpty(Padding) ?
            $"padding-top: {PaddingTop}; " +
            $"padding-right: {PaddingRight}; " +
            $"padding-bottom: {PaddingBottom}; " +
            $"padding-left: {PaddingLeft};" :
            $"padding: {Padding};";
        
        var margin = string.IsNullOrEmpty(Margin)
            ? $"margin-top: {MarginTop};" +
              $"margin-right: {MarginRight};" +
              $"margin-bottom: {MarginBottom};" +
              $"margin-left: {MarginLeft};"
            : $"margin: {Margin};";

        var properties = new[]
        {
            new { Prop = FontSize, CssProp = "font-size" },
            new { Prop = FontWeight, CssProp = "font-weight" },
            new { Prop = TextColor, CssProp = "color" },
            new { Prop = BackgroundColor, CssProp = "background-color" },
        };
        foreach (var cssProp in properties.Where(p => !string.IsNullOrEmpty(p.Prop)))
            styles.Append($"{cssProp.CssProp}: {cssProp.Prop};");
        styles.Append(margin);
        styles.Append(padding);
        return styles.ToString();
    }
}