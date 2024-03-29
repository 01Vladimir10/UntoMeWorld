﻿using UntoMeWorld.Application.Services.Abstractions;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Services;

public class ChurchesService : GenericService<Church>, IChurchesService
{
    public ChurchesService(IChurchesStore store, ILogsService logsService) : base(store, logsService, "Churches")
    {
    }
}