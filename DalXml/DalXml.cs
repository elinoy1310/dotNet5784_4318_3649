namespace Dal;
using DalApi;


sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public DateTime? ProjectStartDate
    {
        get { return Config.ProjectStartDate; }
        set { Config.ProjectStartDate = value; }
    }

    public DateTime? ProjectCompletetDate
    {
        get { return Config.ProjectCompletetDate; }
        set { Config.ProjectCompletetDate = value; }
    }

    private DalXml() { }
}
