
namespace Dal;
/// <summary>
/// DataSource is the entity's data  
/// </summary>
internal static class DataSource
{
    //A collection of entities of a certain type stored as lists in the internal memory 
    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Dependency?> Dependencys { get; } = new();
    internal static List<DO.Task?> Tasks { get; } = new();

    /// <summary>
    /// Config helps define the running variables
    /// </summary>
    internal static class Config
    {
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
       
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }
}

