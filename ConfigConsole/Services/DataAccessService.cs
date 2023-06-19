using Blazored.LocalStorage;
using ConfigConsole.Models;
using System.Text.Json;
using System.Collections.Concurrent;

namespace ConfigConsole.Services;

public class DataAccessService : IDataAccessService
{
    private ILocalStorageService storageService;
    private IDictionary<string, object> datasets;

    public DataAccessService(ILocalStorageService storageService)
    {
        this.storageService = storageService;
        datasets = new ConcurrentDictionary<string, object>();
        datasets[nameof(BatchModel)] = new List<BatchModel>();
        datasets[nameof(PurgeModel)] = new List<PurgeModel>();
        datasets[nameof(TaskModel)] = new List<TaskModel>();
    }

    public async Task InitializeData()
    {
        var hasTasks = await storageService.ContainKeyAsync(nameof(TaskModel));
        if (hasTasks)
        {
            var result = await GetAll<TaskModel>();
            if (result != null)
                datasets[nameof(TaskModel)] = result;
        } else
        {
            datasets[nameof(TaskModel)] = await CreateTaskData();
            await SetLocalValueAsync(nameof(TaskModel), datasets[nameof(TaskModel)]);
        }

        var hasBatches = await storageService.ContainKeyAsync(nameof(BatchModel));
        if (hasBatches)
        {
            var result = await GetAll<BatchModel>();
            if (result != null)
                datasets[nameof(BatchModel)] = result;
        } else
        {
            datasets[nameof(BatchModel)] = await CreateBatchData();
            await SetLocalValueAsync(nameof(BatchModel), datasets[nameof(BatchModel)]);
        }

        var hasPurges = await storageService.ContainKeyAsync(nameof(PurgeModel));
        if (hasPurges)
        {
            var result = await GetAll<PurgeModel>();
            if (result != null)
                datasets[nameof(PurgeModel)] = result;
        } else
        {
            datasets[nameof(PurgeModel)] = await CreatePurgeData();
            await SetLocalValueAsync(nameof(PurgeModel), datasets[nameof(PurgeModel)]);
        }
    }


    // internal storage access methods

    private async Task<T?> GetLocalValueAsync<T>(string key)
    {
        return await storageService.GetItemAsync<T>(key);
    }

    private async Task RemoveLocalValueAsync<T>(string key)
    {
        await storageService.RemoveItemAsync(key);
    }

    private async Task SetLocalValueAsync<T>(string key, T value)
    {
        await storageService.SetItemAsync(key, value);
    }

    // data creation methods
    private async Task<ICollection<BatchModel>> CreateBatchData()
    {
        var batches = new List<BatchModel>();

        // TIMED
        for (int i = 100; i < 110; i++)
        {
            batches.Add(new()
            {
                Id = "DemoBatch" + i.ToString(),
                Enabled = true,
                RequestClass = "com.project.foo",
                ParameterJson = "{}",
                ScheduleType = "TIMED",
                TimeUnit = "MINUTES",
                Quantity = new Random().Next(0, 25),
                LogType = "EXCEPTION"
            });
        }

        // SCHEDULED
        for (int i = 200; i < 220; i++)
        {
            batches.Add(new()
            {
                Id = "DemoBatch" + i.ToString(),
                Enabled = true,
                RequestClass = "com.project.foo",
                ParameterJson = "{}",
                ScheduleType = "SCHEDULED",
                LogType = "EXCEPTION",
                Months = "1,2,3,4,5,6,7,8,9,10,11,12",
                Hours = "11",
                Minutes = "43",
                DayType = "DOW",
                TimeZone = "America/New_York"
            });
        }
        return await Task.FromResult(batches);
    }

    // Task Data

    private async Task<ICollection<TaskModel>> CreateTaskData()
    {
        var tasks = new List<TaskModel>();
        var taskSteps = new List<TaskStep>();

        for (int j = 100; j < 106; j++)
        {
            var anchorId = new Random().Next(1, 100).ToString();
            var quantity = new Random().Next(1, 100);
            taskSteps.Add(new TaskStep()
            {
                Id = "TS" + j.ToString(),
                Status = "PENDING",
                AnchorId = anchorId,
                AnchorPrompt = anchorId,
                Sequence = 0,
                RequiredQuantity = quantity,
                ProcessedQuantity = 0,
                RemainingQuantity = quantity,
                TaskStepExecutions = new List<TaskStepExecution>()
                    {
                        new TaskStepExecution() { Id = "TestHuman"}
                    }
            });
        }

        for (int i = 100; i < 130; i++)
        {
            tasks.Add(new TaskModel
            {
                Id = "T" + i.ToString(),
                Status = "PENDING",
                ProcessData = JsonSerializer.Serialize(new { batchCode = new Random().Next(0, 100), shipmentId = new Random().Next(0, 100000) }),
                ExternalData = JsonSerializer.Serialize(new { department = new Random().Next(0, 10) + "A" }),
                FlowStep = 0,
                WorkflowId = "DemoWorkflow",
                Priority = 0,
                AgingTimestamp = DateTime.UtcNow,
                TaskSteps = taskSteps,
                HumanId = "TestHuman"
            });
        }
        return await Task.FromResult(tasks);
    }

    public async Task<ICollection<PurgeModel>> CreatePurgeData()
    {
        var purges = new List<PurgeModel>();

        for (int i = 100; i < 110; i++)
        {
            purges.Add(new PurgeModel()
            {
                Id = "DemoPurge" + i.ToString(),
                ClassName = "com.demo.foo",
                RetentionDays = 7,
                BulkEnabled = true
            });
        }
        return await Task.FromResult(purges);
    }

    // public data access methods

    public async Task<ICollection<T>> GetAll<T>()
    {
        var dataCached = datasets[typeof(T).Name] as ICollection<T>;
        if (dataCached?.Count > 0)
        {
            return dataCached;
        }
        dataCached = await GetLocalValueAsync<ICollection<T>>(typeof(T).Name);
        return dataCached ?? new List<T>();
    }

    public async Task<T> GetOne<T>(string id) where T : ILocalStorageModel, new()
    {
        var tasks = (await GetAll<T>()).FirstOrDefault(x => x.Id == id);
        return tasks ?? new T();
    }

    public async Task Upsert<T>(T record) where T : ILocalStorageModel
    {
        var items = await GetAll<T>();
        var existingTask = items.FirstOrDefault(x => x.Id == record.Id);
        if (existingTask != null)
        {
            items.Remove(existingTask);
            items.Add(record);
            await SetLocalValueAsync(typeof(T).Name, items);
        }
        else
        {
            items.Add(record);
            await SetLocalValueAsync(typeof(T).Name, items);
        }
    }

    public async Task Remove<T>(string id) where T : ILocalStorageModel
    {
        var tasks = await GetAll<T>();
        var existingTask = tasks.FirstOrDefault(x => x.Id == id);
        if (existingTask != null)
        {
            tasks.Remove(existingTask);
            await SetLocalValueAsync(typeof(T).Name, tasks);
        }
    }

}
