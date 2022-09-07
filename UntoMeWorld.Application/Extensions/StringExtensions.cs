namespace UntoMeWorld.Application.Extensions;

public static class StringExtensions
{
    public static bool In(this string text, params string[] values) =>
        text.In(StringComparison.InvariantCultureIgnoreCase, values);

    private static bool In(this string text, StringComparison comparison, params string[] strings)
        => strings.Any(str => text.Equals(str, comparison));
}