using ConfigConsole.Components;
using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;

namespace ConfigConsole.Pages.General;

public partial class TaskStepExecutions : ComponentBase
{

    // parameters
    [Parameter] public string TaskId { get; set; } = default!;
    [Parameter] public string TaskStepId { get; set; } = default!;

    // services
    [Inject] IDataAccessService DataAccessService { get; set; } = default!;
    [Inject] NavigationManager Navigation { get; set; } = default!;

    private IList<TaskStepExecution> tableData = default!;
    private TaskStep selectedTaskStep = default!;
    private TaskModel selectedTask = default!;

    private ExportButton exportButton = default!;

    protected override async Task OnParametersSetAsync()
    {
        var task = await DataAccessService.GetOne<TaskModel>(TaskId);
        if (task != null)
        {
            selectedTask = task;
            
            if (selectedTask.TaskSteps != null)
            {
                selectedTaskStep = selectedTask.TaskSteps.Find(s => s.Id == TaskStepId) ?? new TaskStep();
                tableData = selectedTaskStep.TaskStepExecutions ?? new();
            } else
            {
                tableData = new List<TaskStepExecution>();
                selectedTaskStep = new();
            }
        }
    }

    // action button commands

    private async Task ExportData()
    {
        await exportButton.DownloadFileFromStreamAsync(tableData);
    }

    private void NavigateToTaskStepView()
    {
        var url = $"general/task/{TaskId}/tasksteps";
        Navigation.NavigateTo(url);
    }

}
