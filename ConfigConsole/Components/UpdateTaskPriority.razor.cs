using ConfigConsole.Models;
using ConfigConsole.Services;
using Microsoft.AspNetCore.Components;

namespace ConfigConsole.Components
{
    public partial class UpdateTaskPriority : ComponentBase
    {
        private int selectedValue = 0;

        [Parameter] public IList<TaskModel> SelectedTasks { get; set; } = default!;
        [Inject] IDataAccessService DataAccess { get; set; } = default!;


        private void UpdateSelectedTask()
        {
            foreach (var task in SelectedTasks)
            {
                task.Priority = selectedValue;
                DataAccess.Upsert(task);
            }
            DialogService.Close(true);
            
        }
    }
}
