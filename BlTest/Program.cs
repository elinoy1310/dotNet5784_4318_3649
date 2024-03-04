// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using BO.Engineer;
using DalApi;
using BlImplementation;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Main method of the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    private static void Main(string[] args)
    {
        try
        {
            // Prompt the user to create initial data.
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            // If user input is 'Y', initialize data.
            if (ans == "Y")
                DalTest.Initialization.Do();

            // Present the main menu.
            presentMainMenu();
        }
        catch (Exception ex)
        {
            // Handle any exceptions and print error message.
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Method to present the main menu.
    /// </summary>
    private static void presentMainMenu()
    {
        bool flagExit = true;
        while (flagExit)
        {
            try
            {

                // Display main menu options
                Console.WriteLine("Select an entity you want to check\r\n0=Exit the main menu\r\n1=Engineer\r\n2=Task\r\n3=Create Schedule");
                // Read user input for main menu option
                string chooseMainMenu = Console.ReadLine()!;
                MainMenu optionMainMenu = (MainMenu)int.Parse(chooseMainMenu);
                // Check selected option
                if (optionMainMenu == MainMenu.Exit)
                    flagExit = false;
                else if (optionMainMenu == MainMenu.Task || optionMainMenu == MainMenu.Engineer)
                    presentSubMenu(optionMainMenu);
                else if(optionMainMenu == MainMenu.CreateSchedule)
                {
                    // Prompt user to enter start project Date
                    Console.WriteLine("enter start project Date:");
                    DateTime start=DateTime.TryParse(Console.ReadLine(), out DateTime st)?st:throw new BlWrongInputFormatException("not a date");
                    s_bl.ProjectStartDate= start;
                    // Prompt user for schedule creation method
                    Console.WriteLine("If you want to create a date manually, then enter an ID-Task , if not enter -1");
                    int taskId=int.Parse(Console.ReadLine()!);
                    if (taskId == -1)
                        s_bl.CreateSchedule();
                    else
                        s_bl.CreateSchedule(BO.Engineer.CreateScheduleOption.Manually,taskId);
                }
                   
                else
                    throw new BlWrongInputFormatException("enter a number between 0-3");
            }
            catch (Exception ex)
            {
                // Handle any exceptions and print error message.
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// Method to present the sub-menu based on the user's selection.
    /// </summary>
    /// <param name="entity">The entity selected in the main menu.</param>
    private static void presentSubMenu(MainMenu entity)
    {
        // Displaying submenu options to the user
        bool flagExit = true;
        while (flagExit)
        {
            try
            {
                Console.WriteLine("Select the method you want to perform:\r\n0=Exit from this menu\r\n1=Adding a new object of the entity type to the list (Create)\r\n2=Display object by ID (Read)\r\n3=View the list of all objects of the entity type (ReadAll)\r\n4=Update existing object data (Update)\r\n5=Deleting an existing object from a list (Delete)");
                // Reading user input for submenu option
                string chooseSubMenu = Console.ReadLine() ?? throw new BlCanNotBeNullException("enter a number");
                SubMenu optionSubMenu = (SubMenu)int.Parse(chooseSubMenu);
                // Perform actions based on user's selection
                switch (optionSubMenu)
                {
                    case SubMenu.exit:
                        flagExit = false;
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
                        throw new BlWrongInputFormatException("enter a number between 0-5");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and print error message
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException);
            }
        }
    }

    private static void readSubMenu(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                // Read Engineer ID from user input
                int engId = inputId();
                Console.WriteLine(s_bl!.Engineer.Read(engId)); // Retrieve and display information about the Engineer with the given ID
                break;

            case MainMenu.Task:
                // Read Task ID from user input
                int taskId = inputId();
                Console.WriteLine(s_bl!.Task.Read(taskId));// Retrieve and display information about the Task with the given ID               
                break;
        }
    }

    /// <summary>
    /// Method to read and display all records of a specified entity from the data source.
    /// </summary>
    /// <param name="entity">The entity type whose records need to be read.</param>
    private static void readAllSubMenu(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                // Iterate through all Engineers and display each record
                foreach (var _engineer in s_bl.Engineer!.ReadAll())
                    Console.WriteLine(_engineer);
                break;
            case MainMenu.Task:
                // Iterate through all Tasks and display each record
                foreach (var _task in s_bl!.Task.ReadAll())
                    Console.WriteLine(_task);
                break;
        }
    }

    /// <summary>
    /// Method to update records of a specified entity based on user input.
    /// </summary>
    /// <param name="entity">The entity type whose records need to be updated.</param>
    private static void updateSubMenu(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                // Update the Engineer record with new user input
                BO.Engineer.Engineer updateEng = newEngineer();
                Console.WriteLine("Enter the ID of the task the engineer working on, enter -1 if you don't want to update a task");
                int taskId = int.Parse(Console.ReadLine() ?? "");
                if(taskId!=-1)
                {
                   
                    updateEng.Task = new BO.Engineer.TaskInEngineer() { Id = taskId };
                    
                }            
                s_bl.Engineer!.Update(updateEng);
                break;
            case MainMenu.Task:
                // Update the Task record with new user input
                Console.WriteLine("enter the task's id you want to update");
                int idTask = int.Parse(Console.ReadLine()??"0");
                s_bl.Task.Read(idTask);
                BO.Engineer.Task updateTask = newTask(idTask);
                Console.WriteLine("Enter the ID of the engineer working on this task, enter -1 if you don't want to update an engineer");
                int engineerId = int.Parse(Console.ReadLine() ?? "");
                if (engineerId != -1)
                {  
                    BO.Engineer.EngineerInTask eng = new BO.Engineer.EngineerInTask() { Id = engineerId };
                    updateTask.Engineer = eng;
                }
                Console.WriteLine("Do you want a start date? (Y/N)");
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans == "Y")
                {
                    Console.WriteLine("Enter Start Date");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                        throw new BlWrongInputFormatException("input not in DateTime format");
                    updateTask.StartDate = startDate;
                }
                // Update the Task record with new user input
                s_bl!.Task.Update(updateTask);
                break;
        }
    }

    /// <summary>
    /// Method to create a new instance of the Engineer class based on user input.
    /// </summary>
    /// <returns>The newly created Engineer instance.</returns>
    private static Engineer newEngineer()
    {
        // Prompt user for Engineer details
        Console.WriteLine("enter the details of the engineer:");
        Console.WriteLine("id:");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("name:");
        string name = Console.ReadLine() ?? "";
        Console.WriteLine("email:");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("level:");
        EngineerExperience level1 = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        // Validate Engineer level
        if ((int)level1 > 4)
            throw new BlWrongInputFormatException($"There is no engineer level for the input number : {(int)level1}");
        Console.WriteLine("cost:");
        double cost = int.Parse(Console.ReadLine() ?? "0");
        // Create a new Engineer instance and return it
        Engineer newEngineer = new Engineer() { Id=id, Name=name, Email=email, Cost=cost, level=level1};
        return newEngineer;
    }

    /// <summary>
    /// Method to create a new instance of the Task class based on user input.
    /// </summary>
    /// <param name="id">Optional parameter representing the ID of the new task.</param>
    /// <returns>The newly created Task instance.</returns>
    private static BO.Engineer.Task newTask(int id=0)
    {
        // Prompt user for Task details
        Console.WriteLine("enter alias,description,required effort time");
        string alias = Console.ReadLine() ?? "";
        string description = Console.ReadLine() ?? "";
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
            throw new BlWrongInputFormatException("input not in TimeSpan format");
        // Initialize list of dependencies
        List<TaskInList> listDep = new List<TaskInList>();
        Console.WriteLine("enter numbers of tasks that this task depend on them, enter -1 in order to stop");
        int numDep = int.Parse(Console.ReadLine() ?? "0");
        while (numDep != -1)
        {
            if (numDep >= 0)
            {
                // Read and add task dependencies
                BO.Engineer.Task? tempTask = s_bl.Task.Read(numDep);
                listDep.Add(new TaskInList() { Id = tempTask.Id, Alias = tempTask.Alias, Description = tempTask.Description, Status = tempTask.Status });
            }
            numDep = int.Parse(Console.ReadLine() ?? "0");
        }
        // Prompt user for additional task details
        Console.WriteLine("enter deliverables,remarks,complexity");
        string deliverables = Console.ReadLine() ?? "";
        string remarks = Console.ReadLine() ?? "";
        EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        if ((int)complexity > 4)
            throw new BlWrongInputFormatException($"There is no task complexity level for the input number : {(int)complexity}");
        // Create and return the new Task instance
        BO.Engineer.Task newTask = new BO.Engineer.Task() {Id=id, Alias=alias, Description=description, RequiredEffortTime=requiredEffortTime, /*ScheduledDate=scheduledDate, CreatedAtDate=createdInDate,Status=status*/ Dependencies=listDep ,/* StartDate=startDate,ForecastDate=forecastDate ,CompleteDate=completeDate*/Deliverables=deliverables, Remarks=remarks,/*Engineer=engineer ,*/Complexity=complexity };  
        return newTask;
    }
    private static void createSubMenu(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                // Create a new Engineer record and display the result
                Console.WriteLine(s_bl.Engineer!.Create(newEngineer()));
                break;
            case MainMenu.Task:
                // Create a new Task record and display the result
                Console.WriteLine(s_bl!.Task.Create(newTask()));
                break;
        }
    }

    private static void deleteSubMenu(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                // Read Engineer ID from user input
                int engId = inputId();
                // Check if the Engineer with the given ID exists, and delete it if found
                s_bl.Engineer.Delete(engId);
                break;
           
            case MainMenu.Task:
                // Read Task ID from user input
                int taskId = inputId();
                // Check if the Task with the given ID exists, and delete it if found
                s_bl.Task.Delete(taskId);
                break;
        }
    }

    /// <summary>
    /// Prompts the user to enter an ID, parses it as an integer, and returns the result.
    /// </summary>
    /// <returns>The parsed integer ID.</returns>
    private static int inputId()
    {
        bool flag;
        Console.WriteLine("enter Id:");
        flag = int.TryParse(Console.ReadLine(), out int id);
        // Check if parsing was successful, otherwise throw an exception
        return flag ? id : throw new BlWrongInputFormatException("not a number");
    }

}
