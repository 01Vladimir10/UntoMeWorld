using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

public class PastorsRepository : GenericServerRepository<Pastor>
{
    public PastorsRepository(HttpClient client) : base(client, "api/pastors")
    {
    }
}