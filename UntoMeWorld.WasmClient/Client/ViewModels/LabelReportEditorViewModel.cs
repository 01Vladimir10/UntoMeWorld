using System.Dynamic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DotLiquid;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Utils.Extensions;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class LabelReportEditorViewModel : BaseViewModel
{
    private readonly ILabelReportsService _service;
    private object? _sampleModel;
    private readonly HttpClient _client;
    private LabelReport _report = new();
    public bool IsLoading { get; set; }

    public LabelReport Report
    {
        get => _report;
        set
        {
            if (Equals(value, _report)) return;
            _report = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Template));
            OnPropertyChanged(nameof(StyleSheet));
        }
    }

    public string Template
    {
        get => Report.Template;
        set
        {
            if (Equals(Report.Template, value))
                return;
            Report.Template = value;
            OnPropertyChanged();
        }
    }

    public object? SampleModel
    {
        get => _sampleModel;
        set
        {
            if (EqualityComparer<object>.Default.Equals(value, _sampleModel)) return;
            _sampleModel = value;
            OnPropertyChanged();
        }
    }

    public string StyleSheet
    {
        get => Report.StyleSheet;
        set
        {
            if (Equals(Report.StyleSheet, value))
                return;
            Report.StyleSheet = value;
            OnPropertyChanged();
        }
    }

    public string RenderedTemplate { get; private set; } = string.Empty;

    public LabelReportEditorViewModel(ILabelReportsService service, HttpClient client)
    {
        _service = service;
        _client = client;
        SetUpListeners();
    }

    private void SetUpListeners()
    {
        this.OnPropsChanged(
                p => p.Template,
                p => p.SampleModel,
                p => p.StyleSheet)
            .Subscribe(UpdateTemplate);
    }

    private void UpdateTemplate()
    {
        if (SampleModel == null)
            return;
        RenderedTemplate = DotLiquid.Template
            .Parse(Template)
            .Render(Hash.FromAnonymousObject(
                new
                {
                    index = 0,
                    item = SampleModel
                }));
    }

    public void Initialize(string reportId)
    {
        if (IsLoading)
            return;
        IsLoading = true;
        Observable
            .FromAsync(async () => await _service.Get(reportId), TaskPoolScheduler.Default)
            .Select(async report =>
            {
                await GetSampleItemAsync(report);
                return report;
            })
            .Concat()
            .SubscribeOn(Scheduler.CurrentThread)
            .Subscribe(report =>
            {
                Report = report ?? new LabelReport();
                UpdateTemplate();
                IsLoading = false;
            });
    }

    private async Task GetSampleItemAsync(LabelReport? report)
    {
        if (report == null)
            return;

        var result = await _client.PostJsonAsync<PaginationResult<ExpandoObject>>(
            $"/api/{report.Collection}/query",
            new QueryRequestDto
            {
                Filter = report.Query,
                OrderBy = report.OrderBy,
                OrderDesc = report.OrderDesc,
                Page = 1,
                PageSize = 1,
                TextQuery = null
            });
        SampleModel = result?.Result?.FirstOrDefault();
    }

    public async Task SaveChangesAsync()
    {
        IsLoading = true;
        await _service.Update(Report);
    }
}