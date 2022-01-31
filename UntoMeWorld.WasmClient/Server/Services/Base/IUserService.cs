using System.Security.Claims;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface IUserService : IDatabaseService<AppUser, string>
{
    public Task<AppUser> GetOrCreateUserByThirdPartyAccountInfo(string authenticationProvider, string thirdPartyUserId, Func<AppUser> onCreateCallback);
}