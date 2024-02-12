// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using BO;
using DalApi;
using BlImplementation;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private static void Main(string[] args)
    {
        //s_bl.Engineer.Create(new BO.Engineer() { Id=89,Cost=55,Name="bbb",Email="aa@gmail.com"});
        //List<TaskInList> lst = new List<TaskInList>();
        //lst.Add(new TaskInList() { Id = 2, Alias = "ttt", Description = "hhh", Status = BO.Status.Scheduled });

        //s_bl.Task.Add(new BO.Task() { Id = 3, Dependencies = lst ,Alias="ggg"});
        //Console.WriteLine( s_bl.Task.Read(1));
        try
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
          
            presentMainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static void presentMainMenu()
    {
        bool flagExit = true;
        while (flagExit)
        {
            try
            {

                Console.WriteLine("Select an entity you want to check\r\n0=Exit the main menu\r\n1=Engineer\r\n2=Task\r\n3=CREATE SCHEDULE");
                // Read user input for main menu option
                string chooseMainMenu = Console.ReadLine()!;
                MainMenu optionMainMenu = (MainMenu)int.Parse(chooseMainMenu);
                if (optionMainMenu == MainMenu.Exit)
                    flagExit = false;
                else if (optionMainMenu == MainMenu.Task || optionMainMenu == MainMenu.Engineer)
                    presentSubMenu(optionMainMenu);
                else if((int)optionMainMenu==3)
                    s_bl.CreateSchedule();
                else
                    throw new BlWrongInputFormatException("enter a number between 0-3");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

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

    private static void updateSubMenu(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                // Update the Engineer record with new user input
                BO.Engineer updateEng = newEngineer();
                Console.WriteLine("Enter the ID of the task the engineer working on, enter -1 if you don't want to update a task");
                int choice = int.Parse(Console.ReadLine() ?? "");
                if(choice!=-1)
                {
                    int? taskId = int.Parse(Console.ReadLine() ?? "");
                    updateEng.Task = new BO.TaskInEngineer() { Id = taskId };
                }            
                s_bl.Engineer!.Update(updateEng);
                break;
            case MainMenu.Task:
                // Update the Task record with new user input
                Console.WriteLine("enter the task's id you want to update");
                int idTask = int.Parse(Console.ReadLine()??"0");
                s_bl.Task.Read(idTask);
                BO.Task updateTask = newTask(idTask);
                Console.WriteLine("Enter the ID of the engineer working on this task, enter -1 if you don't want to update an engineer");
                int choice1 = int.Parse(Console.ReadLine() ?? "");
                if (choice1 != -1)
                {
                    int engineerId = int.Parse(Console.ReadLine() ?? "0");
                    BO.EngineerInTask eng = new BO.EngineerInTask() { Id = engineerId };
                    updateTask.Engineer = eng;
                }           
                // Update the Dependency record with new user input               
                s_bl!.Task.Update(updateTask);
                break;
        }
    }

    private static Engineer newEngineer()
    {
        //user input
        Console.WriteLine("enter the details of the engineer:");
        Console.WriteLine("id:");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("name:");
        string name = Console.ReadLine() ?? "";
        Console.WriteLine("email:");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("level:");
        EngineerExperience level1 = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("cost:");
        double cost = int.Parse(Console.ReadLine() ?? "0");
        ////Console.WriteLine("idTask:");
        ////int idTask = int.Parse(Console.ReadLine() ?? "0");
        ////Console.WriteLine("aliasTask:");
        ////string aliasTask = Console.ReadLine() ?? "";
        ////TaskInEngineer task= new TaskInEngineer() { Id=idTask, Alias=aliasTask };

        //creates new Engineer and returns it
        Engineer newEngineer = new Engineer() { Id=id, Name=name, Email=email, Cost=cost, level=level1};
        return newEngineer;
    }

    private static BO.Task newTask(int id=0)
    {
        //user input
        Console.WriteLine("enter alias,description,required effort time");
        string alias = Console.ReadLine() ?? "";
        string description = Console.ReadLine() ?? "";
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
            throw new BlWrongInputFormatException("input not in TimeSpan format");
        //if (DateTime.TryParse(Console.ReadLine(), out DateTime createdInDate))
        //    throw new BlWrongInputFormatException("input not in DateTime format");
        //Status status = (Status)int.Parse(Console.ReadLine() ?? "0");
        List<TaskInList> listDep = new List<TaskInList>();
        Console.WriteLine("enter numbers of tasks that this task depend on them, enter -1 in order to stop");
        int numDep = int.Parse(Console.ReadLine() ?? "0");
        while (numDep != -1)
        {
            if (numDep >= 0)
            {
                BO.Task? tempTask = s_bl.Task.Read(numDep);
                listDep.Add(new TaskInList() { Id = tempTask.Id, Alias = tempTask.Alias, Description = tempTask.Description, Status = tempTask.Status });
            }
            numDep = int.Parse(Console.ReadLine() ?? "0");
        }
        Console.WriteLine("enter deliverables,remarks,complexity");
        //if (DateTime.TryParse(Console.ReadLine(), out DateTime scheduledDate))
        //    throw new BlWrongInputFormatException("input not in DateTime format");
        //if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        //    throw new BlWrongInputFormatException("input not in DateTime format");
        //if (DateTime.TryParse(Console.ReadLine(), out DateTime forecastDate))
        //    throw new BlWrongInputFormatException("input not in DateTime format");
        //if (DateTime.TryParse(Console.ReadLine(), out DateTime completeDate))
        //    throw new BlWrongInputFormatException("input not in DateTime format");
        // Console.WriteLine("enter num of dependency, to finish enter -1");

        string deliverables = Console.ReadLine() ?? "";
        string remarks = Console.ReadLine() ?? "";
        EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        //create new Task and returns it
        BO.Task newTask = new BO.Task() {Id=id, Alias=alias, Description=description, RequiredEffortTime=requiredEffortTime, /*ScheduledDate=scheduledDate, CreatedAtDate=createdInDate,Status=status*/ Dependencies=listDep ,/* StartDate=startDate,ForecastDate=forecastDate ,CompleteDate=completeDate*/Deliverables=deliverables, Remarks=remarks,/*Engineer=engineer ,*/Complexity=complexity };  
        return newTask;
    }
    // dependencies 
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

    private static int inputId()
    {
        bool flag;
        Console.WriteLine("enter Id:");
        flag = int.TryParse(Console.ReadLine(), out int id);
        return flag ? id : throw new BlWrongInputFormatException("not a number");
    }

}
