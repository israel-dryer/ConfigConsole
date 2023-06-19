using ConfigConsole.Components;
using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;

namespace ConfigConsole.Pages.General
{
    public partial class BatchView : ComponentBase
    {
        // services
        [Inject] IDataAccessService DataAccessService { get; set; } = default!;
        [Inject] DialogService DialogService { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        // object references
        private RadzenDataGrid<BatchModel> dataGrid = default!;
        private ExportButton exportButton = default!;

        // page data
        private ICollection<BatchModel> tableData = default!;
        private IList<BatchModel> selectedRecords = default!;
        public IList<BatchModel> SelectedRecords
        {
            get => selectedRecords;
            set
            {
                selectedRecords = value;
                actionButtonDisabled = false;
            }
        }


        // other page properties
        private bool actionButtonDisabled = true;

        protected async override Task OnInitializedAsync()
        {
            tableData = await DataAccessService.GetAll<BatchModel>();
            selectedRecords = new List<BatchModel>();
        }

        // action button commands
        private async void DeleteRecord()
        {
            var record = selectedRecords.First();
            if (record == null) { return; }
            if (record?.Id == null) { return; }

            // update source data
            await DataAccessService.Remove<BatchModel>(record.Id);

            // update table data
            tableData.Remove(record);
            await dataGrid.Reload();
            actionButtonDisabled = true;
        }

        private void NavigateToCreateRecordForm()
        {
            Navigation.NavigateTo("/general/batch/batches/new");
        }

        private void NavigateToEditRecordForm()
        {
            var record = selectedRecords.First();

            if (record == null) { return; }
            Navigation.NavigateTo($"/general/batch/batches/edit/{record.Id}");
        }

        private async Task ExportRecordsToCsv()
        {
            await exportButton.DownloadFileFromStreamAsync(tableData);
        }

        private async Task ExecuteBatch(object _)
        {
            var record = selectedRecords.First();
            if (record == null) { return; }

            var options = new AlertOptions() { OkButtonText = "OK" };
            await DialogService.Alert($"Execution instructions for {record.Id}", "Execute Batch", options);
        }

    }
}
