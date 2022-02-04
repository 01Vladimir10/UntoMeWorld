namespace UntoMeWorld.WasmClient.Server.Services.Options;

public class AuthorizationServiceOptions
{
    public List<string> ReadActions { get; set; }
    public List<string> DeleteActions { get; set; }
    public List<string> UpdateActions { get; set; }
    public List<string> AddActions { get; set; }
    public List<string> RestoreActions { get; set; }
    public List<string> PurgeActions { get; set; }
    public List<string> SpecialActions { get; set; }
    public string PermissionSelectionMode { get; set; }
    public string RoleSelectionMode { get; set; }
}
