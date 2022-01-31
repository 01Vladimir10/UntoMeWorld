using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Security.Authentication;

public static class AuthEventsHandler
{
    public static async Task OnAzureAdTokenValidated(TokenValidatedContext context, IServiceProvider provider)
    {

    }

}