using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

public class ChildrenRepository : GenericServerRepository<Child>
{
    public ChildrenRepository(HttpClient client) : base(client, "api/children")
    {
        
    }
}