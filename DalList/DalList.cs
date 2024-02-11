﻿namespace Dal;
using DalApi;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? ProjectStartDate
    {
        get { return DataSource.Config.ProjectStartDate; }
        set { DataSource.Config.ProjectStartDate = value; }
    }

    public DateTime? ProjectCompletetDate
    {
        get { return DataSource.Config.ProjectCompletetDate; }
        set { DataSource.Config.ProjectCompletetDate = value; }
    }

    private DalList() { }

}
