using Dal;
using DalApi;
using DO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace DalTest
{

    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

       
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                PresentMainMenu();
                
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }
        //אם יהיה זמן להוריד את הסוויץ
        public static void PresentMainMenu()
        {
            try
            {
                bool flagExit = true;
                while (flagExit)
                {
                    Console.WriteLine("Select an entity you want to check\r\n0= Exit the main menu\r\n1= engineer\r\n2=dependency\r\n3=task");
                    string chooseMainManu = Console.ReadLine()!;
                    MainMenu optionMainMenu = (MainMenu)int.Parse(chooseMainManu);
                    if(optionMainMenu!=MainMenu.Exit)
                    PresentSubMenu(optionMainMenu);
                    else 
                        flagExit= false;
                    //switch (optionMainManu)
                    //{
                    //    case MainMenu.Exit:
                    //        flagExit = false;
                    //        break;
                    //    case MainMenu.Engineer:
                    //        PresentSubMenu(MainMenu.Engineer);
                    //        break;
                    //    case MainMenu.Dependency:
                    //        PresentSubMenu(MainMenu.Dependency);
                    //        break;
                    //    case MainMenu.Task:
                    //        PresentSubMenu(MainMenu.Task);
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            
        }



        public static void PresentSubMenu(MainMenu entity)
        {
            string chooseSubManu = Console.ReadLine()??"0";
            SubMenu optionSubMain = (SubMenu)int.Parse(chooseSubManu);
            bool flag = true;
            while(flag)
            {
                switch (optionSubMain)
                {
                    case SubMenu.exit:flag = false;
                        break;
                    case SubMenu.create: Create(entity);
                        break;
                    case SubMenu.read: ReadSubMenu(entity);
                        break;
                    case SubMenu.readAll: ReadAllSubMenu(entity);
                        break;
                    case SubMenu.update:UpdateSubMenu(entity);
                        break;
                    case SubMenu.delete:DeleteSubMenu(entity);
                        break;
                    default:
                        break;
                }
            }
           
        }

        /// <summary>
        /// Creates a new Dependency object by taking user input for Id, Dependent, and DependsOnTask.
        /// </summary>
        /// <remarks>
        /// This method prompts the user to enter values for Id, Dependent, and DependsOnTask to
        /// create a new Dependency object. If the user provides invalid input or leaves the input
        /// blank, default values of 0 are used for Id, Dependent, and DependsOnTask.
        /// </remarks>
        /// <returns>A new Dependency object based on user input.</returns>
        public static Dependency NewDependency()
        {
            // Prompt the user to enter Id, Dependent, and DependsOnTask for the new Dependency
            Console.WriteLine("Enter Id, Dependent, DependsOnTask");
            // Parse user input for Dependent and set default to 0 if input is null or invalid
            int depDependent = int.Parse(Console.ReadLine() ?? "0");
            // Parse user input for DependsOnTask and set default to 0 if input is null or invalid
            int depDependsOnTask = int.Parse(Console.ReadLine() ?? "0");
            // Create a new Dependency object with Id set to 0 and user-input Dependent and DependsOnTask
            Dependency dep = new Dependency(0, depDependent, depDependsOnTask);
            // Return the created Dependency object
            return dep;
        }

        /// <summary>
        /// Reads and displays information about a specific entity (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which information will be read.</param>
        public static void ReadSubMenu(MainMenu entity)
        {
            switch (entity)
            {
                //case MainMenu.Exit:
                //    // No action needed if the main menu option is Exit
                //    return;
                //break;
                case MainMenu.Engineer:
                    // Read Engineer ID from user input
                    int engId = int.Parse(Console.ReadLine()!);
                    if (s_dalEngineer!.Read(engId) is not null)// Retrieve and display information about the Engineer with the given ID
                        Console.WriteLine(s_dalEngineer!.Read(engId));
                    else
                        throw new Exception($"Engineer with ID = {engId} was not found");
                    break;
                case MainMenu.Dependency:
                    // Read Dependency ID from user input
                    int depId = int.Parse(Console.ReadLine()!);
                    if (s_dalDependency!.Read(depId) is not null)// Retrieve and display information about the Dependency with the given ID
                        Console.WriteLine(s_dalDependency!.Read(depId));
                    else
                        throw new Exception($"Dependency with ID = {depId} was not found");
                    break;
                case MainMenu.Task:
                    // Read Task ID from user input
                    int taskId = int.Parse(Console.ReadLine()!);
                    if (s_dalTask!.Read(taskId) is not null)// Retrieve and display information about the Task with the given ID
                        Console.WriteLine(s_dalTask!.Read(taskId));
                    else
                        throw new Exception($"Task with ID = {taskId} was not found");
                    break;
                //default:
                //    // No action needed for other MainManu options
                //    break;
            }
        }

        /// <summary>
        /// Reads and displays all records for a specific entity (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which all records will be read and displayed.</param>
        public static void ReadAllSubMenu(MainMenu entity)
        {
            switch (entity)
            {
                case MainMenu.Exit:
                    // No action needed if the main menu option is Exit
                    return;
                //break;
                case MainMenu.Engineer:
                    // Iterate through all Engineers and display each record
                    foreach (var _engineer in s_dalEngineer!.ReadAll())
                        Console.WriteLine(_engineer);
                    break;
                case MainMenu.Dependency:
                    // Iterate through all Dependencies and display each record
                    foreach (var _dependency in s_dalDependency!.ReadAll())
                        Console.WriteLine(_dependency);
                    break;
                case MainMenu.Task:
                    // Iterate through all Tasks and display each record
                    foreach (var _task in s_dalTask!.ReadAll())
                        Console.WriteLine(_task);
                    break;
                default:
                    // No action needed for other MainManu options
                    break;
            }
        }

        /// <summary>
        /// Updates a specific record (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainMenu entity for which a record will be updated.</param>
        public static void UpdateSubMenu(MainMenu entity)
        {
            // Display existing record details before updating
            ReadSubMenu(entity);
            switch (entity)
            {
                //case MainMenu.Exit:
                //    return;
                ////break;
                case MainMenu.Engineer:
                    // Update the Engineer record with new user input
                    s_dalEngineer!.Update(NewEngineer());
                    break;
                case MainMenu.Dependency:
                    // Update the Dependency record with new user input
                    s_dalDependency!.Update(NewDependency());
                    break;
                case MainMenu.Task:
                    // Update the Task record with new user input
                    s_dalTask!.Update(NewTask());   
                    break;
                default:
                    // No action needed for other MainMenu options
                    break;
            }
        }


        /// <summary>
        /// This function prompts the user to enter data in order to create an Engineer object.
        /// </summary>
        /// <returns>An Engineer object with the data entered by the user.</returns>
        public static Engineer NewEngineer()
        {
            //user input
            Console.WriteLine("enter id,email,cost,name and level of the engineer");
            int id = int.Parse(Console.ReadLine() ?? "0");
            string email = Console.ReadLine() ?? "";
            double cost = int.Parse(Console.ReadLine() ?? "0");
            string name = Console.ReadLine() ?? "";
            EngineerExperience level = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");

            //creates new Engineer and returns it
            Engineer newEngineer = new Engineer(id, email, cost, name, level);
            return newEngineer;
        }

        /// <summary>
        /// This function prompts the user to enter data in order to create an Task object.
        /// </summary>
        /// <returns>A Task object with the data entered by the user.</returns>
        public static DO.Task NewTask()
        {
            //user input
            Console.WriteLine("enter alias,description,if the task is mile stone=false, required effort time, created in date,scheduled date=null,start date, complete date, dead line, deliverables,remarks,engineer id, engineer's experience,complexity");
            string alias = Console.ReadLine() ?? "";
            string description = Console.ReadLine() ?? "";
            bool isMileStone;
            bool flag = bool.TryParse(Console.ReadLine(), out isMileStone);
            isMileStone = flag ? isMileStone : false;
            TimeSpan requiredEffortTime;
            flag = TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime);
            requiredEffortTime = flag ? requiredEffortTime : TimeSpan.Zero;
            DateTime createdInDate;
            flag = DateTime.TryParse(Console.ReadLine(), out createdInDate);
            createdInDate = flag ? createdInDate : DateTime.Today;
            DateTime scheduledDate;
            flag = DateTime.TryParse(Console.ReadLine(), out scheduledDate);
            scheduledDate = flag ? scheduledDate : DateTime.Today;
            DateTime startDate;
            flag = DateTime.TryParse(Console.ReadLine(), out startDate);
            startDate = flag ? startDate : DateTime.Today;
            DateTime completeDate;
            flag = DateTime.TryParse(Console.ReadLine(), out completeDate);
            completeDate = flag ? completeDate : DateTime.Today;
            DateTime deadline;
            flag = DateTime.TryParse(Console.ReadLine(), out deadline);
            deadline = flag ? deadline : DateTime.Today;
            string deliverables = Console.ReadLine() ?? "";
            string remarks = Console.ReadLine() ?? "";
            int engineerId = int.Parse(Console.ReadLine() ?? "");
            EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
            //create new Task and returns it
            DO.Task newTask = new DO.Task(0, alias, description, isMileStone, requiredEffortTime, createdInDate, scheduledDate, startDate, completeDate, deadline, deliverables, remarks, engineerId, complexity);
            return newTask;
        }

        /// <summary>
        /// Creates a new record (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainMenu entity for which a new record will be created.</param>
        public static void Create(MainMenu entity)
        {
            //try
            //{

                switch (entity)
                {
                    case MainMenu.Engineer:
                    // Create a new Engineer record and display the result
                    Console.WriteLine(s_dalEngineer!.Create(NewEngineer()));
                        break;
                    case MainMenu.Dependency:
                    // Create a new Dependency record and display the result
                    Console.WriteLine(s_dalDependency!.Create(NewDependency()));
                        break;
                    case MainMenu.Task:
                    // Create a new Task record and display the result
                    Console.WriteLine(s_dalTask!.Create(NewTask()));


                        break;
                    default:
                    // No action needed for other MainMenu options
                    break;
                }
            
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        /// <summary>
        /// Deletes a specific record (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which a record will be deleted.</param>
        public static void DeleteSubMenu(MainMenu entity)
        {
            switch (entity)
            {
                case MainMenu.Exit:
                    // No action needed if the main menu option is Exit
                    return;
                //break;
                case MainMenu.Engineer:
                    // Read Engineer ID from user input
                    int engId = int.Parse(Console.ReadLine()!);
                    // Check if the Engineer with the given ID exists, and delete it if found
                    if (s_dalEngineer!.Read(engId) is not null)
                        s_dalEngineer.Delete(engId);
                    else
                        throw new Exception($"Engineer with ID = {engId} was not found");
                    break;
                case MainMenu.Dependency:
                    // Read Dependency ID from user input
                    int depId = int.Parse(Console.ReadLine()!);
                    // Check if the Dependency with the given ID exists, and delete it if found
                    if (s_dalDependency!.Read(depId) is not null)
                        s_dalDependency.Delete(depId);
                    else
                        throw new Exception($"Dependency with ID = {depId} was not found");
                    break;
                case MainMenu.Task:
                    // Read Task ID from user input
                    int taskId = int.Parse(Console.ReadLine()!);
                    // Check if the Task with the given ID exists, and delete it if found
                    if (s_dalTask!.Read(taskId) is not null)
                        s_dalTask.Delete(taskId);
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
    
    

    


















