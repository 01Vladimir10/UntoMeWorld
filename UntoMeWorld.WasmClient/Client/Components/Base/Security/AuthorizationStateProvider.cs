using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Client.Data.Repo.Base;
using UntoMeWorld.WasmClient.Client.Security;
using UntoMeWorld.WasmClient.Shared.Security.Utils;

namespace UntoMeWorld.WasmClient.Client.Components.Base.Security;

public class AuthorizationStateProvider : ComponentBase, IAuthorizationProvider
{
    [Inject] public IPermissionsRepository PermissionsRepo { get; set; }
    [Inject] public AuthenticationStateProvider AuthenticationState { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    public AppUser CurrentUser { get; private set; }
    public Dictionary<string, Permission> CurrentUserPermissions { get; private set; } = new();

    private bool _isAuthenticated;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        AuthenticationState.AuthenticationStateChanged += OnAuthenticationStateChanged;
        OnAuthenticationStateChanged(AuthenticationState.GetAuthenticationStateAsync());
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> stateTask)
    {
        Console.WriteLine("AuthenticationStateChanged");
        var state = await stateTask;
        if (state.User.Identity is not { IsAuthenticated: true })
        {
            CurrentUser = null;
            _isAuthenticated = false;
            return;
        }
        _isAuthenticated = state.User.Identity.IsAuthenticated;
        CurrentUser = state.User.Claims.ToAppUser();
        var permissions = await PermissionsRepo.GetCurrentUsersPermissions();
        CurrentUserPermissions = permissions.ToDictionary(p => p.Key.ToUpper(), p => p.Value);
    }
    public bool IsAuthorized(string resource, PermissionType requiredPermission)
    {
        if (!_isAuthenticated || CurrentUser == null)
            return false;
        resource = resource.Trim().ToUpper();
        if (CurrentUserPermissions.ContainsKey(resource))
            return EvaluateRequiredPermission(CurrentUserPermissions[resource], requiredPermission);
        return CurrentUserPermissions.ContainsKey("*") &&
               EvaluateRequiredPermission(CurrentUserPermissions["*"], requiredPermission);
    }

    public Task<bool> IsAuthorizedAsync(string resource, PermissionType requiredPermission)
        => Task.FromResult(IsAuthorized(resource, requiredPermission));


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<IAuthorizationProvider>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
        base.BuildRenderTree(builder);
    }
    
    private static bool EvaluateRequiredPermission(Permission permission, PermissionType permissionType)
    {
        return permissionType switch
        {
            PermissionType.Add => permission.Add,
            PermissionType.Delete => permission.Delete,
            PermissionType.Update => permission.Update,
            PermissionType.Read => permission.Read,
            PermissionType.Restore => permission.Restore,
            PermissionType.Purge => permission.Purge,
            PermissionType.Special => permission.Special,
            PermissionType.Unknown => false,
            _ => false
        };
    }

}