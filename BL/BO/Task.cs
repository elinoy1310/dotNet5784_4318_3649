

using DO;

namespace BO;

public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime? CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public IEnumerable<TaskInList>? Dependencies { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience Complexity { get; set; }
    public override string ToString() => this.ToStringProperty();


}
