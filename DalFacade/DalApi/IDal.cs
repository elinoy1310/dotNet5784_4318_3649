
namespace DalApi;
public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    IUser User { get; }
    public DateTime? ProjectStartDate { get; set; }
   // public DateTime? ProjectCompletetDate { get; set; }
}
