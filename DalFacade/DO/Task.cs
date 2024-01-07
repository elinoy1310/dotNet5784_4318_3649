using System.ComponentModel;

namespace DO;

public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedInDate,
    EngineerExperience Complexity,
    TimeSpan RequiredEffortTime,
    bool IsMileStone,
    DateTime StartDate,
    DateTime ScheduledDate,
    DateTime Deadline,
    DateTime CompleteDate,
    string Deliverables,
    string Remarks,
    int EngineerId   
)
{

}
