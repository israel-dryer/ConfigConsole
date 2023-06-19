namespace ConfigConsole.Models;

public class BatchModel : ILocalStorageModel
{
    public string? Id { get; set; }
    public string? RequestClass { get; set; }
    public string? ParameterJson { get; set; } = "{}";
    public string? ScheduleType { get; set; } = "SCHEDULED";
    public string? Months { get; set; }
    public string? Days { get; set; }
    public string? Hours { get; set; }
    public string? Minutes { get; set; }
    public string? DayType { get; set; }
    public string? TimeZone { get; set; }
    public string? TimeUnit { get; set; }
    public int? Quantity { get; set; }
    public bool Enabled { get; set; }
    public string? LogType { get; set; } = "EXCEPTION";

}
