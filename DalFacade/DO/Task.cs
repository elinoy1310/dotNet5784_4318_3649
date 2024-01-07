using System.ComponentModel;

namespace DO;

public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedInDate,
    TimeSpan RequiredEffortTime,
    bool IsMileStone,
    DateTime StartDate,
    DateTime 
    
)
{

}
