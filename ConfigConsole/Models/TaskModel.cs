using System.Text.Json.Serialization;

namespace ConfigConsole.Models
{
    public class TaskModel : ILocalStorageModel
    {
        [JsonPropertyName("taskId")]
        public string? Id { get; set; }

        [JsonPropertyName("taskType")]
        public string? TaskType { get; set;}

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("processData")]
        public string? ProcessData { get; set; }

        [JsonPropertyName("externalData")]
        public string? ExternalData { get; set; }

        [JsonPropertyName("flowStep")]
        public int FlowStep { get; set; }

        [JsonPropertyName("area")]
        public string? Area { get; set; }

        [JsonPropertyName("valueStream")]
        public string? ValueStream { get; set; }

        [JsonPropertyName("externalGroup")]
        public string? ExternalGroup { get; set; }

        [JsonPropertyName("processGroup")]
        public string? ProcessGroup { get; set; }

        [JsonPropertyName("altId")]
        public string? AltId { get; set; }

        [JsonPropertyName("startAfterTimestamp")]
        public DateTime? StartAfterTimestamp { get; set; }

        [JsonPropertyName("slaTimestamp")]
        public DateTime? SlaTimestamp { get; set; }

        [JsonPropertyName("startByTimestamp")]
        public DateTime? StartByTimestamp { get; set; }

        [JsonPropertyName("endByTimestamp")]
        public DateTime? EndByTimestamp { get; set; }

        [JsonPropertyName("projectedStartTimestamp")]
        public DateTime? ProjectedStartTimestamp { get; set; }

        [JsonPropertyName("projectedEndTimestamp")]
        public DateTime? ProjectedEndTimestamp { get; set; }

        [JsonPropertyName("modelingGroup")]
        public string? ModelingGroup { get; set; }

        [JsonPropertyName("workflowId")]
        public string? WorkflowId { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("humanId")]
        public string? HumanId { get; set; }

        [JsonPropertyName("agingTimestamp")]
        public DateTime? AgingTimestamp { get; set; }

        [JsonPropertyName("reported")]
        public bool Reported { get; set; }

        [JsonPropertyName("taskSteps")]
        public List<TaskStep>? TaskSteps { get; set; }
    }

    public class TaskStep : ILocalStorageModel
    {
        [JsonPropertyName("taskStepId")]
        public string? Id { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("anchorId")]
        public string? AnchorId { get; set; }

        [JsonPropertyName("anchorPrompt")]
        public string? AnchorPrompt { get; set; }

        [JsonPropertyName("materialId")]
        public string? MaterialId { get; set; }

        [JsonPropertyName("materialPrompt")]
        public string? MaterialPrompt { get; set; }

        [JsonPropertyName("sequence")]
        public int? Sequence { get; set; }

        [JsonPropertyName("requiredQuantity")]
        public int? RequiredQuantity { get; set; }

        [JsonPropertyName("processedQuantity")]
        public int? ProcessedQuantity { get; set; }

        [JsonPropertyName("remainingQuantity")]
        public int? RemainingQuantity { get; set; }

        [JsonPropertyName("processData")]
        public string? ProcessData { get; set; }

        [JsonPropertyName("externalData")]
        public string? ExternalData { get; set; }

        [JsonPropertyName("lot")]
        public string? Lot { get; set; }

        [JsonPropertyName("sourcePrompt")]
        public string? SourcePrompt { get; set; }

        [JsonPropertyName("destPrompt")]
        public string? DestPrompt { get; set; }

        [JsonPropertyName("unitOfMeasure")]
        public string? UnitOfMeasure { get; set; }

        [JsonPropertyName("taskStepExecutions")]
        public List<TaskStepExecution>? TaskStepExecutions { get; set; }
    }

    public class TaskStepExecution : ILocalStorageModel
    {
        public string? Id { get; set; }
        public string? Lot { get; set; }
        public string? SourceId { get; set;}
        public string? DestinationId { get; set; }
        public DateTime? ExecutionDate { get; set; } = DateTime.Now;
        public bool Reported { get; set; } = false;
        public string? ProcessData { get; set; }
    }

}
