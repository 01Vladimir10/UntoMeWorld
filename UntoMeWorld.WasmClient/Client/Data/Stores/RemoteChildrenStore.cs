using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public class RemoteChildrenStore : GenericRemoteStore<Child>, IChildrenStore
{
    public RemoteChildrenStore(HttpClient client) : base(client, "/api/children")
    {
    }
    
}