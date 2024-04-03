using System.ComponentModel;

namespace DO;

/// <summary>
/// Task Entity represents a task with all its details
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="Alias">short,unique name</param>
/// <param name="Description">text that describes the task</param>
/// <param name="RequiredEffortTime">number of days required in order to complete the task</param>
/// <param name="CreatedInDate">the time when the task was created by the manager</param>
/// <param name="ScheduledDate">planned date for the start of the work so that all tasks are completed by the time the project is finished</param>
/// <param name="StartDate">when an engineer begins the actual work on the task</param>
/// <param name="CompleteDate">when an engineer reports that he finished the task</param>
/// <param name="Deliverables">a string that describes the results or the items provided at the end of the task </param>
/// <param name="Remarks">remarks on the task(optional)</param>
/// <param name="EngineerId"> the id of the engineer that works on the task</param>
/// <param name="Complexity">dhe complexity level of the task, defines the minimal engineer level for the engineer to work on the task</param>
public record Task
(
    int Id,
    string? Alias=null,
    string? Description=null,
    TimeSpan? RequiredEffortTime=null,
    DateTime? CreatedInDate=null,   
    DateTime? ScheduledDate=null,
    DateTime? StartDate=null,   
    DateTime? CompleteDate=null,
    string? Deliverables=null,
    string? Remarks=null,
    int? EngineerId=null,
    EngineerExperience Complexity=EngineerExperience.Beginner
)
{
    public Task() : this(Id: 0, Alias: "", Description: "", CreatedInDate: DateTime.Now) { }
    public bool ShouldSerializeScheduledDate() { return ScheduledDate.HasValue; }
    public bool ShouldSerializeStartDate() { return StartDate.HasValue; }
    public bool ShouldSerializeRequiredEffortTime() { return RequiredEffortTime.HasValue; }
    public bool ShouldSerializeCompleteDate() { return CompleteDate.HasValue; }
    public bool ShouldSerializeDeliverables() { return !string.IsNullOrEmpty(Deliverables); }
    public bool ShouldSerializeRemarks() { return !string.IsNullOrEmpty(Remarks); }
    public bool ShouldSerializeEngineerId() { return EngineerId.HasValue; }
}