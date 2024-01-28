namespace Dal;
using DalApi;


sealed public class DalXml : IDal
{
    public static DalXml Instance = new DalXml();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public DalXml() { }
}
