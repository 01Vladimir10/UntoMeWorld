using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Repo.Base;

public interface IPermissionsRepository
{
    public Task<Dictionary<string, Permission>> GetCurrentUsersPermissions();
}