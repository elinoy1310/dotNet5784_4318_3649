namespace BO;

    public class GanttTask
    {
    public int TaskId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CompleteDate { get; set; }
    public BO.Status Status { get; set; }
    public int? EngineerId { get; set; }
    public string? TaskAlias { get; set; }
    public string? EngineerName { get; set; }
    public IEnumerable<int>? DependentTasks { get; set; }
}



