using Microsoft.AspNetCore.Components;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Client.Services.Security;

namespace UntoMeWorld.WasmClient.Client.Components.Base.Security;

public abstract class AuthorizedControl : ComponentBase, IAuthorizedControl
{
    [CascadingParameter] public ApiResource CascadingApiResource { get; set; } = ApiResource.Unknown;
    [Parameter] public ApiResource ApiResource { get; set; } = ApiResource.Unknown;
    [Parameter] public PermissionType RequiredPermission { get; set; }
    [Inject] public IAuthorizationProviderService AuthorizationProvider { get; set; }
    private ApiResource EffectiveResource => ApiResource == ApiResource.Unknown ? CascadingApiResource : ApiResource;
    protected bool IsAuthorized { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        IsAuthorized = await AuthorizationProvider.ChallengeAsync(EffectiveResource, RequiredPermission);
    }
    
}