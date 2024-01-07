using System.ComponentModel;

namespace DO;

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

}
