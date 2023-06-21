using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;

namespace ConfigConsole.Pages.General
{
    public partial class BatchNewEdit : ComponentBase
    {

        // services
        [Inject] private IDataAccessService DataAccess { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        // page parameters
        [Parameter]
        public string BatchId { get; set; } = default!;

        // other page properties
        private string pageTitle = "Batch Create Form";
        private bool deleteButtonVisible = false;

        // form data
        public List<string> BatchTypes { get; private set; }
        public List<string> DayTypes { get; private set; }
        public List<string> TimeUnits { get; private set; }
        public List<string> LogTypes { get; private set; }
        public BatchModel Batch { get; set; }



        public BatchNewEdit()
        {
            Batch = new();
            BatchTypes = new() { "SCHEDULED", "TIMED" };
            DayTypes = new() { "DOW", "DOM", "WOM" };
            TimeUnits = new() { "NANOSECONDS", "MICROSECONDS", "MILLISECONDS", "SECONDS", "MINUTES", "HOURS", "DAYS" };
            LogTypes = new() { "ALL", "NONE", "EXCEPTION" };

        }

        protected async override Task OnParametersSetAsync()
        {
            if (BatchId is not null)
            {
                Batch = await DataAccess.GetOne<BatchModel>(BatchId);
                deleteButtonVisible = true;
                pageTitle = "Batch Edit Form";
                deleteButtonVisible = true;
            }
            else
            {
                Batch = new();
                pageTitle = "Batch Create Form";
            }
        }


        private async Task UpdateBatch()
        {
            if (!string.IsNullOrEmpty(Batch.Id)) 
            {
                await DataAccess.Upsert(Batch);
            }
            Navigation.NavigateTo("general/batch/batches");
        }

        private async Task RemoveBatch()
        {
            await DataAccess.Remove<BatchModel>(BatchId);
            Navigation.NavigateTo("general/batch/batches");

        }
    }
}
