
namespace Dal;
using DO;
/// <summary>
/// DataSource is the entity's data  
/// </summary>
internal static class DataSource
{
    //each entity saved in list in the internal memory 
    internal static List<Engineer?> Engineers { get; } = new();
    internal static List<Dependency?> Dependencys { get; } = new();
    internal static List<Task?> Tasks { get; } = new();
    internal static List<User?> Users { get; } = new();


    /// <summary>
    /// Config helps define the running variables
    /// </summary>
    internal static class Config
    {
        //define running variable for Task.id
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        //define running variable for Dependency.id
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        public static DateTime? ProjectStartDate { get; set; }
        public static DateTime? ProjectCompletetDate { get; set; }

    }
}

