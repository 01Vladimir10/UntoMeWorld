using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

public class ChurchesRepository : GenericServerRepository<Church>
{
    public ChurchesRepository(HttpClient client) : base(client, "api/churches")
    {
    }
}