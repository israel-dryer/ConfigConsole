namespace ConfigConsole.Models;

public class LaborEventModel
{
    public int? LaborEventId { get; set; }
    public string? LaborEvent { get; set; }
    public int ActualMinutes { get; set; } = 0;
    public int StandardMinutes { get; set; } = 0;
    public int Quantity { get; set; } = 0;
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? EndTime { get; set; }
    public string? HumanId { get; set; }
    public string? ReportingId { get; set; }
    public string? WorkflowId { get; set; }
    public string? DeviceId { get; set; }
}
