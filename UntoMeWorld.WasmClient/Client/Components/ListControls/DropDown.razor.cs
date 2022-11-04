using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.ListControls;
public static class DropDowns
{
    public static readonly IReadOnlyList<DropDownOption<bool>> Boolean = new List<DropDownOption<bool>>
    {
        new(true, "Yes"), 
        new(false, "No")
    };

    public static List<DropDownOption<T>> FromEnum<T>() where T :  struct, Enum
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("T is not an Enum type");
        return Enum.GetValues<T>().Select(e => new DropDownOption<T>(e, Enum.GetName(e))).ToList();
    }
}