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
                presentMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// isplays the main menu and allows the user to select an entity to check or exit the program.
        /// </summary>
        private static void presentMainMenu()
        {          
             bool flagExit = true;
             while (flagExit)
             {
                try
                {
                    Console.WriteLine("Select an entity you want to check\r\n0= Exit the main menu\r\n1= engineer\r\n2=dependency\r\n3=task");
                    // Read user input for main menu option
                    string chooseMainMenu = Console.ReadLine()!;
                    MainMenu optionMainMenu = (MainMenu)int.Parse(chooseMainMenu);

                    // Check if the user chose to exit the main menu
                    if (optionMainMenu != MainMenu.Exit ||(int)optionMainMenu>3)
                        // Present the submenu based on the selected main menu option
                        presentSubMenu(optionMainMenu);
                    else 
                        flagExit= false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             }
           
        }

        /// <summary>
        ///  Displays a submenu based on the selected entity type(Engineer, Dependency, or Task) and allows the user to perform various operations.
       /// </summary>
        /// <param name="entity">The main menu option representing the entity type.</param>
        private static void presentSubMenu(MainMenu entity)
        {
           
         // Displaying submenu options to the user
           bool flag = true;
           while (flag)
           { 
                try
                {
                    Console.WriteLine("Select the method you want to perform:\r\n0=Exit from this menu\r\n1=Adding a new object of the entity type to the list (Create)\r\n2=Display object by ID (Read)\r\n3=View the list of all objects of the entity type (ReadAll)\r\n4=Update existing object data (Update)\r\n5=Deleting an existing object from a list (Delete)");
                    // Reading user input for submenu option
                    string chooseSubMenu = Console.ReadLine() ?? "0";
                    SubMenu optionSubMenu = (SubMenu)int.Parse(chooseSubMenu);
                    // Performing the selected operation based on the submenu option
                    switch (optionSubMenu)
                    {
                        case SubMenu.exit:
                            flag = false;
                            break;
                        case SubMenu.create:
                            createSubMenu(entity);
                            break;
                        case SubMenu.read:
                            readSubMenu(entity);
                            break;
                        case SubMenu.readAll:
                            readAllSubMenu(entity);
                            break;
                        case SubMenu.update:
                            updateSubMenu(entity);
                            break;
                        case SubMenu.delete:
                            deleteSubMenu(entity);
                            break;
                        default:
                            throw new Exception("enter a number between 0-5");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
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
        private static Dependency newDependency()
        {
            // Prompt the user to enter Dependent, and DependsOnTask for the new Dependency
            Console.WriteLine("Enter Dependent:");
            // Parse user input for Dependent and set default to 0 if input is null or invalid
            int depDependent = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Enter DependsOnTask:");
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
        private static void readSubMenu(MainMenu entity)
        {           
            switch (entity)
            {
                case MainMenu.Engineer:
                    // Read Engineer ID from user input
                    Console.WriteLine("enter Id:");
                    int engId = int.Parse(Console.ReadLine()!);
                    if (s_dalEngineer!.Read(engId) is not null)// Retrieve and display information about the Engineer with the given ID
                        Console.WriteLine(s_dalEngineer!.Read(engId));
                    else
                        throw new Exception($"Engineer with ID = {engId} was not found");
                    break;
                case MainMenu.Dependency:
                    // Read Dependency ID from user input
                    Console.WriteLine("enter Id:");
                    int depId = int.Parse(Console.ReadLine()!);
                    if (s_dalDependency!.Read(depId) is not null)// Retrieve and display information about the Dependency with the given ID
                        Console.WriteLine(s_dalDependency!.Read(depId));
                    else
                        throw new Exception($"Dependency with ID = {depId} was not found");
                    break;
                case MainMenu.Task:
                    // Read Task ID from user input
                    Console.WriteLine("enter Id:");
                    int taskId = int.Parse(Console.ReadLine()!);
                    if (s_dalTask!.Read(taskId) is not null)// Retrieve and display information about the Task with the given ID
                        Console.WriteLine(s_dalTask!.Read(taskId));
                    else
                        throw new Exception($"Task with ID = {taskId} was not found");
                    break;
            }
        }
       
        /// <summary>
        /// Reads and displays all records for a specific entity (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which all records will be read and displayed.</param>
        private static void readAllSubMenu(MainMenu entity)
        {
            switch (entity)
            {
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
            }
        }

        /// <summary>
        /// Updates a specific record (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainMenu entity for which a record will be updated.</param>
        private static void updateSubMenu(MainMenu entity)
        {
            // Display existing record details before updating          
            readSubMenu(entity);
            switch (entity)
            {
                case MainMenu.Engineer:
                    // Update the Engineer record with new user input
                    s_dalEngineer!.Update(newEngineer());
                    break;
                case MainMenu.Dependency:
                    Console.WriteLine("enter the dependentcy's id you want to uppdate");
                    int idDependency =int.Parse(Console.ReadLine()!);
                     // Update the Dependency record with new user input
                    s_dalDependency!.Update(newDependency() with { Id = idDependency });
                    break;
                case MainMenu.Task:
                    // Update the Task record with new user input
                    Console.WriteLine("enter the task's id you want to uppdate");
                    int idTask =int.Parse(Console.ReadLine()!);
                    // Update the Dependency record with new user input
                    s_dalTask!.Update(newTask() with { Id = idTask });   
                    break;
            }
        }

        /// <summary>
        /// This function prompts the user to enter data in order to create an Engineer object.
        /// </summary>
        /// <returns>An Engineer object with the data entered by the user.</returns>
        private static Engineer newEngineer()
        {
            //user input
            Console.WriteLine("enter the details of the engineer:");
            Console.WriteLine("id:");
            int id = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("email:");
            string email = Console.ReadLine() ?? "";
            Console.WriteLine("cost:");
            double cost = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("name:");
            string name = Console.ReadLine() ?? "";
            Console.WriteLine("level:");
            EngineerExperience level = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");

            //creates new Engineer and returns it
            Engineer newEngineer = new Engineer(id, email, cost, name, level);
            return newEngineer;
        }

        /// <summary>
        /// This function prompts the user to enter data in order to create an Task object.
        /// </summary>
        private static DO.Task newTask()
        {
            //user input
            Console.WriteLine("enter alias,description,if the task is mile stone=false, required effort time, created in date,scheduled date=null,start date, complete date, dead line, deliverables,remarks,engineer id, engineer's experience,complexity");
            string alias = Console.ReadLine() ?? "";
            string description = Console.ReadLine() ?? "";
            if (!bool.TryParse(Console.ReadLine(), out bool isMileStone))
                throw new Exception("wrong input format");
            if(!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
                throw new Exception("wrong input format");
            if(DateTime.TryParse(Console.ReadLine(), out DateTime createdInDate))
                throw new Exception("wrong input format");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime scheduledDate))
                throw new Exception("wrong input format");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                throw new Exception("wrong input format");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime completeDate))
                throw new Exception("wrong input format");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
                throw new Exception("wrong input format");
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
        private static void createSubMenu(MainMenu entity)
        {
                switch (entity)
                {
                    case MainMenu.Engineer:
                    // Create a new Engineer record and display the result
                    Console.WriteLine(s_dalEngineer!.Create(newEngineer()));
                        break;
                    case MainMenu.Dependency:
                    // Create a new Dependency record and display the result
                    Console.WriteLine(s_dalDependency!.Create(newDependency()));
                        break;
                    case MainMenu.Task:
                    // Create a new Task record and display the result
                    Console.WriteLine(s_dalTask!.Create(newTask()));
                    break;
                }
        }

        /// <summary>
        /// Deletes a specific record (Engineer, Dependency, or Task) based on user input.
        /// </summary>
        /// <param name="entity">The MainManu entity for which a record will be deleted.</param>
        private static void deleteSubMenu(MainMenu entity)
        {
            switch (entity)
            {
                case MainMenu.Engineer:
                    // Read Engineer ID from user input
                    Console.WriteLine("enter Id:");
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
            }
        }
    }
}
    
    

    


















