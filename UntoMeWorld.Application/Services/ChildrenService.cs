using UntoMeWorld.Application.Services.Abstractions;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Services;

public class ChildrenService : GenericService<Child>, IChildrenService
{
    public ChildrenService(IChildrenStore store, ILogsService logsService) : base(store, logsService, "Children")
    {
    }
}