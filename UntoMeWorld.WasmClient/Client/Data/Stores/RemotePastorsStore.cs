using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public class RemotePastorsStore : GenericRemoteStore<Pastor>, IPastorsStore
{
    public RemotePastorsStore(HttpClient client) : base(client, "/api/pastors")
    {
    }
}