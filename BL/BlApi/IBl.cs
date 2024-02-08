using BO;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public  ProjectStatus CheckProjectStatus();

}
