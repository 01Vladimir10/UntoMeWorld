using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Components.Children;
using UntoMeWorld.WasmClient.Client.Components.Dialogs;
using UntoMeWorld.WasmClient.Client.Components.Interop;
using UntoMeWorld.WasmClient.Client.Data.Model;

namespace UntoMeWorld.WasmClient.Client.Pages.Children;

[Authorize]
public class ChildrenPageBase : ComponentBase
{

}