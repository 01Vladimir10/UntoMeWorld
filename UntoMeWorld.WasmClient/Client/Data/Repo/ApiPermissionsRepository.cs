using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Data.Common;
using UntoMeWorld.WasmClient.Client.Data.Repo.Base;
using UntoMeWorld.WasmClient.Client.Utils;

namespace UntoMeWorld.WasmClient.Client.Data.Repo;

public class ApiPermissionsRepository : IPermissionsRepository
{
    private readonly HttpClient _client;

    public ApiPermissionsRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<Dictionary<string, Permission>> GetCurrentUsersPermissions()
    {
        var response = await _client.GetJsonAsync<Dictionary<string, Permission>>(ApiRoutes.Roles.GetCurrentUserPermissions);
        if (response.IsSuccessful)
            return response.Data;
        throw new Exception(response.ErrorMessage);
    }
}