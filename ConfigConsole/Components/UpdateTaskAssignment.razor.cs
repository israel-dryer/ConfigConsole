using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;

namespace ConfigConsole.Components;

public partial class UpdateTaskAssignment : ComponentBase
{
    private IList<string> humanIds = default!;
    private string selectedHuman = string.Empty;

    [Parameter] public IList<TaskModel> SelectedTasks { get; set; } = default!;
    [Inject] IDataAccessService DataAccess { get; set; } = default!;

    protected async override Task OnParametersSetAsync()
    {
        humanIds = new List<string>
        {
            "DemoUser1", "DemoUser2", "DemoUser3", "DemoUser4", "DemoUser5",
            "DemoUser6", "DemoUser7", "DemoUser8", "DemoUser9", "DemoUser10"
        };
    }

    private async Task UpdateSelectedTask()
    {
        if (!string.IsNullOrEmpty(selectedHuman))
        {
            foreach(var task in SelectedTasks)
            {
                task.HumanId = selectedHuman;
                await DataAccess.Upsert(task);
            }

        }
        DialogService.Close(true);
    }
}


