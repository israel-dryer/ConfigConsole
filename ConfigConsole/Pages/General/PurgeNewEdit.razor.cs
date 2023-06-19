using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;

namespace ConfigConsole.Pages.General
{
    public partial class PurgeNewEdit : ComponentBase
    {

        // services
        [Inject] private IDataAccessService DataAccess { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        // page parameters
        [Parameter]
        public string PurgeId { get; set; } = default!;

        // form data
        public PurgeModel Purge { get; set; } = default!;

        // other page properties
        private string pageTitle = "Purge Create Form";
        private bool deleteButtonIsVisible = false;


        protected async override Task OnParametersSetAsync()
        {
            
            
            if (PurgeId is not null)
            {
                Purge = await DataAccess.GetOne<PurgeModel>(PurgeId);
                deleteButtonIsVisible = true;
                pageTitle = "Purge Edit Form";
                
            } else
            {
                Purge = new();
            }
        }

        private async Task UpdatePurge()
        {
            if (!string.IsNullOrEmpty(Purge.Id))
                await DataAccess.Upsert(Purge);
            Navigation.NavigateTo("/general/batch/purges");
        }

        private async Task RemovePurge()
        {
            await DataAccess.Remove<PurgeModel>(PurgeId);
            Navigation.NavigateTo("/general/batch/purges");
        }
    }
}
