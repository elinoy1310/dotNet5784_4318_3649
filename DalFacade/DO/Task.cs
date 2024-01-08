using System.ComponentModel;

namespace DO;

/// <summary>
/// Task Entity represents a task with all its details
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="Alias">short,unique name</param>
/// <param name="Description">text that describes the task</param>
/// <param name="IsMileStone">does the task is a mile stone right now</param>
/// <param name="RequiredEffortTime">number of days required in order to complete the task</param>
/// <param name="CreatedInDate">the time when the task was created by the manager</param>
/// <param name="ScheduledDate">planned date for the start of the work so that all tasks are completed by the time the project is finished</param>
/// <param name="StartDate">when an engineer begins the actual work on the task</param>
/// <param name="CompleteDate">when an engineer reports that he finished the task</param>
/// <param name="Deadline">the latest possible date on which the completion of the task will not cause the project to fail</param>
/// <param name="Deliverables">a string that describes the results or the items provided at the end of the task </param>
/// <param name="Remarks">remarks on the task(optional)</param>
/// <param name="EngineerId"> the id of the engineer that works on the task</param>
/// <param name="Complexity">dhe complexity level of the task, defines the minimal engineer level for the engineer to work on the task</param>
public record Task
(
    int Id,
    string? Alias=null,
    string? Description=null,
    bool IsMileStone=false, 
    TimeSpan? RequiredEffortTime=null,
    DateTime? CreatedInDate=null,   
    DateTime? ScheduledDate=null,
    DateTime? StartDate=null,   
    DateTime? CompleteDate=null,
    DateTime? Deadline=null,
    string? Deliverables=null,
    string? Remarks=null,
    int? EngineerId=null,
    EngineerExperience Complexity=EngineerExperience.Beginner
)
{
    //a parameterized constructor is automatially present
    public Task():this(0) { } //empty constructor
}
