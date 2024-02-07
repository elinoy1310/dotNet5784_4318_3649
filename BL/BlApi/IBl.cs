using BO;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public  DateTime? ProjectStartDate { get; init; }
    public  DateTime? ProjectCompletetDate { get; init; }
    public  ProjectStatus CheckProjectStatus();

}
