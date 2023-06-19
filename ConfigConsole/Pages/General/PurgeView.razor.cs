using ConfigConsole.Components;
using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace ConfigConsole.Pages.General;

public partial class PurgeView : ComponentBase
{
    // services
    [Inject] IDataAccessService DataAccessService { get; set; } = default!;
    [Inject] NavigationManager Navigation { get; set; } = default!;
    [Inject] DialogService DialogService { get; set; } = default!;

    // object references
    private RadzenDataGrid<PurgeModel> dataGrid = default!;
    private ExportButton exportButton = default!;
    private IList<PurgeModel> selectedRecords = default!;
    public IList<PurgeModel> SelectedRecords
    {
        get => selectedRecords;
        set
        {
            selectedRecords = value;
            actionButtonDisabled = false;
        }
    }

    //  page data
    private ICollection<PurgeModel> tableData = default!;

    // view properties
    private bool actionButtonDisabled = true;


    protected async override Task OnInitializedAsync()
    {
        tableData = await DataAccessService.GetAll<PurgeModel>();
        selectedRecords = new List<PurgeModel>();    
    }

    // action button commands
    private void NavigateToCreateRecordForm()
    {
        Navigation.NavigateTo("/general/batch/purges/new");
    }

    private void NavigateToEditRecordForm()
    {
        var record = selectedRecords.First();
        if (record == null) { return; }
        Navigation.NavigateTo($"/general/batch/purges/edit/{record.Id}");
    }

    private async Task DeleteRecord()
    {
        var record = selectedRecords.First();
        if (record is not null && !string.IsNullOrEmpty(record.Id))
        {
            // update source data
            await DataAccessService.Remove<BatchModel>(record.Id);

            // update table
            tableData.Remove(record);
            await dataGrid.Reload();
        }
        actionButtonDisabled = true;
    }

    private async Task ExportRecordsToCsv()
    {
        await exportButton.DownloadFileFromStreamAsync(tableData);
    }

    private async Task ExecuteBatch(object _)
    {
        var record = selectedRecords.First();
        if (record== null) { return; }

        var options = new AlertOptions() { OkButtonText = "OK" };
        await DialogService.Alert($"Execution instructions for {record.Id}", "Execute Batch", options); 
    }
}
