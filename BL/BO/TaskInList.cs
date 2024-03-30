

using BlImplementation;

namespace BO;

public class TaskInList
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public Status Status { get; set; }
    public override string ToString() => this.ToStringProperty();
    public bool Select(BO.Task task)
    {
        if (task.Dependencies?.Count() == 0)
            return false;
        foreach(var dep in task.Dependencies!)
            if(this.Id == dep.Id)
                return true;
        return false;
    }

}
