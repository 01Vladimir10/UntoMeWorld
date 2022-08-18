using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.Utils.Extensions;

public static class ModelExtensions
{
    public static string FullName(this Child child)
        => $"{child.Name} ${child.Lastname}";
}