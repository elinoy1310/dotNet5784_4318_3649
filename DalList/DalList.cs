namespace Dal;
using DalApi;

sealed public class DalList : IDal
{
    
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
}
