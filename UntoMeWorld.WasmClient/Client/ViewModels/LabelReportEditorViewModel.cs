using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DotLiquid;
using UntoMeWorld.Application.Model;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class LabelReportEditorViewModel<T> : BaseViewModel
{
    private readonly ILabelReportsService _service;
    public bool IsLoading { get; set; }
    public LabelReport? Report { get; set; }

    public LabelReportEditorViewModel(ILabelReportsService service)
    {
        _service = service;
    }

    public string RenderedTemplate
    {
        get
        {
            if (Report == null)
                return "";
            var template = Template.Parse(Report.Template);
        }
    }

    private void SetUpCallbacks()
    {
        
    }
    
    
    
    public void Refresh(string reportId)
    {
        if (IsLoading)
            return;
        IsLoading = true;
        Observable
            .FromAsync(async () => await _service.Get(reportId),
                TaskPoolScheduler.Default)
            .SubscribeOn(Scheduler.CurrentThread)
            .Subscribe(report =>
            {
                Report = report ?? new LabelReport();
                IsLoading = false;
            });
    }
    
    
    public async Task SaveChangesAsync()
    {
        if (Report != null)
        {
            IsLoading = true;
            await _service.Update(Report);
        }
    }
}