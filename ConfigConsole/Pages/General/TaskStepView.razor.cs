using ConfigConsole.Components;
using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;

namespace ConfigConsole.Pages.General
{
    public partial class TaskStepView : ComponentBase
    {
        // services
        [Inject] IDataAccessService DataAccessService { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        // parameters
        [Parameter] public string TaskId { get; set; } = string.Empty;

        // table data
        private ICollection<TaskStep> tableData = default!;
        private TaskModel selectedTask = default!;

        // object references
        private ExportButton exportButton { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            var task = await DataAccessService.GetOne<TaskModel>(TaskId);
            if (task != null)
            {
                selectedTask = task;
                tableData = task?.TaskSteps ?? new List<TaskStep>();
            }
            else
            {
                selectedTask = new TaskModel();
                tableData = new List<TaskStep>();
            }
        }

        // action buttons

        private async Task ExportData()
        {
            await exportButton.DownloadFileFromStreamAsync(tableData);
        }

        private void NavigateToTaskView()
        {
            Navigation.NavigateTo("/general/task");
        }

        private void NavigateToTaskStepExecutionView(string? taskId, string? taskStepId)
        {
            var url = $"/general/task/{taskId}/tasksteps/{taskStepId}/executions";
            Navigation.NavigateTo(url);
        }
    }
}
