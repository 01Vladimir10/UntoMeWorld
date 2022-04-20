namespace UntoMeWorld.WasmClient.Client.Utils.UIHelpers;

public static class RazorUtils
{
    public static T If<T>(bool condition, T value)
    {
        return condition ? value : default;
    }
    public static string BoolToString(bool value)
    {
        return value ? "Yes" : "No";
    }
}