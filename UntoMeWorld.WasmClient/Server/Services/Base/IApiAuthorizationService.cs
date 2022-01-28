namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface IApiAuthorizationService
{
    public Task<bool> ValidateUserAuthenticatedRequest(string userId, string controller, string action);
    public Task<bool> ValidateTokenAuthenticatedRequest(string token, string controller, string action);
}