using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.Icons;

public class PhosphorIconBase : Icon
{
    private string WeightClass => Weight switch
    {
        IconWeight.Regular => "",
        IconWeight.Thin => "-thin",
        IconWeight.Light => "-light",
        IconWeight.Bold => "-bold",
        IconWeight.Fill => "-fill",
        _ => ""
    };
    protected string AllCssClasses => $"{IconName}{WeightClass} {CssClass}";

    public PhosphorIconBase()
    {
        
    }
    public PhosphorIconBase(string iconName)
    {
        IconName = iconName;
    }
    public override string ToString()
    {
        return $"<{Element} class=\"{AllCssClasses}\"> </{Element}>";
    }

    private string Element => HtmlElement switch
    {
        HtmlElement.Div => "div",
        HtmlElement.Button => "button",
        HtmlElement.Span => "span",
        _ => "i"
    };
}