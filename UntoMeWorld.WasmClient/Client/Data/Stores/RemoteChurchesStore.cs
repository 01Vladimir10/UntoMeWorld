using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public class RemoteChurchesStore : GenericRemoteStore<Church>, IChurchesStore
{
    public RemoteChurchesStore(HttpClient client, string endPoint) : base(client, endPoint)
    {
        
    }
}