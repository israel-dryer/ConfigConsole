using ConfigConsole.Components;
using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;


namespace ConfigConsole.Pages.General
{
    public partial class TaskView : ComponentBase
    {

        // services
        [Inject] IDataAccessService DataAccessService { get; set; } = default!;
        [Inject] DialogService DialogService {get;set;} = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;
        

        // button controls (at last one task must be selected)
        private bool actionButtonDisabled = true;

        // object references
        private RadzenDataGrid<TaskModel> taskDataGrid = default!;
        private ExportButton exportButton = default!;

        // datagrid data references
        private ICollection<TaskModel> tableData = default!;
        private IList<TaskModel> selectedTasks = default!;
        
        public IList<TaskModel> SelectedTasks
        {
            get => selectedTasks;
            set
            {
                selectedTasks = value;
                if (selectedTasks.Count == 0)
                    actionButtonDisabled = true;
                else
                    actionButtonDisabled = false;
            }
        }

        protected async override Task OnInitializedAsync()
        {
            tableData = await DataAccessService.GetAll<TaskModel>();
            selectedTasks = new List<TaskModel>();
        }

        // action button commands
        private async Task ExportTaskData()
        {
            await exportButton.DownloadFileFromStreamAsync(tableData);
        }

        private async Task UpdateTaskAssignment()
        {
            if (selectedTasks.Count == 0) { return; }

            // create settings for user selection dialog
            var args = new Dictionary<string, object>() { { "selectedTasks", selectedTasks } };
            var options = new DialogOptions() { Width = "20rem" };
            await DialogService.OpenAsync<UpdateTaskAssignment>("Task Assignment", args, options);

            

            tableData = await DataAccessService.GetAll<TaskModel>();
            await taskDataGrid.Reload();
        }

        private async Task UpdateTaskPriority()
        {
            if (selectedTasks.Count == 0) { return; }

            var args = new Dictionary<string, object>() { { "selectedTasks", selectedTasks } };
            var options = new DialogOptions() { Width = "15rem" };
            await DialogService.OpenAsync<UpdateTaskPriority>("Task Priority", args, options);
            await taskDataGrid.Reload();
        }

        private async Task DeleteTask()
        {
            if (selectedTasks.Count == 0) { return; }

            foreach (var task in selectedTasks)
            {
                if (task?.Id != null)
                {
                    await DataAccessService.Remove<TaskModel>(task.Id);
                    tableData.Remove(task);
                }
            }
            await taskDataGrid.Reload();
            actionButtonDisabled = true;
        }

        private async Task ResetTask(object _)
        {
            var options = new AlertOptions() { OkButtonText = "OK" };
            await DialogService.Alert("Resetting selected tasks!", "Reset", options);
        }

        private void ShowTaskSteps(string? taskId)
        {
            Navigation.NavigateTo($"general/task/{taskId}/taskSteps");
        }
    }


}
