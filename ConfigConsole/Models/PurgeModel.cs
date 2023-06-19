namespace ConfigConsole.Models;

public class PurgeModel : ILocalStorageModel
{
    public string? Id { get; set; }
    public string? ClassName { get; set; }
    public int? RetentionDays { get; set; }
    public bool BulkEnabled { get; set; }
}
