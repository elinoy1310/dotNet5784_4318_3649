namespace Dal;
using DalApi;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    private DalList() { }

}
