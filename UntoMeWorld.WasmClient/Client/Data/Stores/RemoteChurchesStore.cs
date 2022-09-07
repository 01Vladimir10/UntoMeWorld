using UntoMeWorld.Domain.Model;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.WasmClient.Shared.DTOs.Church;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public class RemoteChurchesStore : GenericRemoteStore<Church, ChurchDto, UpdateChurchDto>, IChurchesStore
{
    public RemoteChurchesStore(HttpClient client) : base(client, "/api/churches")
    {
        
    }

    protected override ChurchDto ToAddDto(Church model) => new ChurchDto().From(model);

    protected override UpdateChurchDto ToUpdateDto(Church model) => new UpdateChurchDto().From(model);
}