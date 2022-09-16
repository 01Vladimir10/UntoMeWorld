namespace UntoMeWorld.WasmClient.Client.Utils.Extensions;

public  static class StringExtensions
{
    public static string Truncate(this string text, int maxLength)
        => text.Length > maxLength ? $"{text[..maxLength]}..." : text;

}