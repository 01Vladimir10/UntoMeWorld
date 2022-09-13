using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Services.Base;

public interface IUserService : IService<AppUser, string>
{
    public Task<AppUser> GetOrCreateUserByThirdPartyAccountInfo(string authenticationProvider, string thirdPartyUserId, Func<AppUser> onCreateCallback);
    public Task<bool> IsDisabled(string tokenId);
}