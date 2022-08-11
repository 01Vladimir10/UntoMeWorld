using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Shared.DTOs.Children;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public class RemoteChildrenStore : GenericRemoteStore<Child, ChildDto, UpdateChildDto>, IChildrenStore
{
    public RemoteChildrenStore(HttpClient client) : base(client, "/api/children")
    {
    }

    protected override ChildDto ToAddDto(Child model) => new ChildDto().From(model);
    protected override UpdateChildDto ToUpdateDto(Child model) => new UpdateChildDto().From(model);
}