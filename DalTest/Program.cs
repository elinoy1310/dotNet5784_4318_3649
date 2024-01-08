using Dal;
using DalApi;
using DO;

namespace DalTest
{

    internal class Program
    {
        private static ITask? s_dalTask=new TaskImplementation();
        private static IEngineer? s_dalEndineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEndineer,s_dalTask,s_dalDependency);

            }

            catch(Exception ex) 
            {

                Console.WriteLine(ex.Message);
            }
            
        }

        public void PresentMainManu()
        {
            Console.WriteLine("Select an entity you want to check\r\n0= Exit the main menu\r\n1= engineer\r\n2=dependency\r\n3=task");
            string chooseMainManu = Console.ReadLine()!;
            MainManu optionMainManu = (MainManu)int.Parse(chooseMainManu);
            switch (optionMainManu)
            {
                case MainManu.Exit:
                    break;
                case MainManu.Engineer:
                    break;
                case MainManu.Dependency:
                    break;
                case MainManu.Task:
                    break;
                default:
                    break;
            }
        }

        

        public void PresentSubMenu(MainManu entity)
        {
            string chooseSubManu = Console.ReadLine()!;
            SubManu optionSubMain = (SubManu)int.Parse(chooseSubManu);
            switch (optionSubMain)
            {
                case SubManu.exit:
                    break;
                case SubManu.create:
                    break;
                case SubManu.read:
                    break;
                case SubManu.readAll:
                    break;
                case SubManu.update:
                    break;
                case SubManu.delete:
                    break;
                default:
                    break;
            }


        }

        public Dependency NewDependency()
        {
            Console.WriteLine("Enter Id, Dependent, DependsOnTask");
            int depDependent = int.Parse(Console.ReadLine()?? "0");
            int depDependsOnTask=int.Parse(Console.ReadLine()?? "0");   
            Dependency dep = new Dependency(0, depDependent, depDependsOnTask);
            return dep; 
        }

        /// <summary>
        /// Reads and displays information about a specific entity (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which information will be read.</param>
        public void ReadSubManu(MainManu entity)
        {
            switch (entity)
            {
                case MainManu.Exit:
                    // No action needed if the main menu option is Exit
                    return;
                    //break;
                case MainManu.Engineer:
                    // Read Engineer ID from user input
                    int engId =int.Parse(Console.ReadLine()!);
                    if (s_dalEndineer!.Read(engId) is not null)// Retrieve and display information about the Engineer with the given ID
                        Console.WriteLine(s_dalEndineer!.Read(engId));
                    else
                        throw new Exception($"Engineer with ID = {engId} was not found");
                    break;
                case MainManu.Dependency:
                    // Read Dependency ID from user input
                    int depId = int.Parse(Console.ReadLine()!);
                    if (s_dalDependency!.Read(depId) is not null)// Retrieve and display information about the Dependency with the given ID
                        Console.WriteLine(s_dalDependency!.Read(depId));
                    else
                        throw new Exception($"Dependency with ID = {depId} was not found");
                    break;
                case MainManu.Task:
                    // Read Task ID from user input
                    int taskId = int.Parse(Console.ReadLine()!);
                    if (s_dalTask!.Read(taskId) is not null)// Retrieve and display information about the Task with the given ID
                        Console.WriteLine(s_dalTask!.Read(taskId));
                    else
                        throw new Exception($"Task with ID = {taskId} was not found");
                    break;
                default:
                    // No action needed for other MainManu options
                    break;
            }
        }

        /// <summary>
        /// Reads and displays all records for a specific entity (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which all records will be read and displayed.</param>
        public void ReadAllSubManu(MainManu entity)
        {
            switch (entity)
            {
                case MainManu.Exit:
                    // No action needed if the main menu option is Exit
                    return;
                //break;
                case MainManu.Engineer:
                    // Iterate through all Engineers and display each record
                    foreach (var _engineer in s_dalEndineer!.ReadAll())
                        Console.WriteLine(_engineer);
                    break;
                case MainManu.Dependency:
                    // Iterate through all Dependencies and display each record
                    foreach (var _dependency in s_dalDependency!.ReadAll())
                        Console.WriteLine(_dependency);
                    break;
                case MainManu.Task:
                    // Iterate through all Tasks and display each record
                    foreach (var _task in s_dalTask!.ReadAll())
                        Console.WriteLine(_task);
                    break;
                default:
                    // No action needed for other MainManu options
                    break;
            }
        }

        public void UpdateSubManu(MainManu entity)
        {
            switch (entity)
            {
                case MainManu.Exit:
                    return;
                //break;
                case MainManu.Engineer:
                    ReadSubManu(entity);
                    
                    break;
                case MainManu.Dependency:
                    ReadSubManu(entity);

                    break;
                case MainManu.Task:
                    ReadSubManu(entity);

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Deletes a specific record (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which a record will be deleted.</param>
        public void DeleteSubManu(MainManu entity)
        {
            switch (entity)
            {
                case MainManu.Exit:
                    // No action needed if the main menu option is Exit
                    return;
                //break;
                case MainManu.Engineer:
                    // Read Engineer ID from user input
                    int engId = int.Parse(Console.ReadLine()!);
                    // Check if the Engineer with the given ID exists, and delete it if found
                    if (s_dalEndineer!.Read(engId) is not null)
                        s_dalEndineer.Delete(engId);
                    else
                        throw new Exception($"Engineer with ID = {engId} was not found");
                    break;
                case MainManu.Dependency:
                    // Read Dependency ID from user input
                    int depId = int.Parse(Console.ReadLine()!);
                    // Check if the Dependency with the given ID exists, and delete it if found
                    if (s_dalDependency!.Read(depId) is not null)
                        s_dalDependency.Delete(depId); 
                    else
                        throw new Exception($"Dependency with ID = {depId} was not found");
                    break;
                case MainManu.Task:
                    // Read Task ID from user input
                    int taskId = int.Parse(Console.ReadLine()!);
                    // Check if the Task with the given ID exists, and delete it if found
                    if (s_dalTask!.Read(taskId) is not null)
                        s_dalTask.Delete(taskId );
                    else
                        throw new Exception($"Task with ID = {taskId} was not found");
                    break;
                default:
                    // No action needed for other MainManu options
                    break;
            }
        }

    }
}

















