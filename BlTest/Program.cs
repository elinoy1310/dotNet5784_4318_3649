// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using BO;
using DalApi;


internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private static void Main(string[] args)
    {
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
    }

    private static void presentSubMenu(MainMenu entity)
    {
    }

    private static void readSubMenu(MainMenu entity)
    {
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
        readSubMenu(entity);
        switch (entity)
        {
            case MainMenu.Engineer:
                // Update the Engineer record with new user input
                s_bl.Engineer!.Update(newEngineer());
                break;
            case MainMenu.Task:
                // Update the Task record with new user input
                Console.WriteLine("enter the task's id you want to update");
                int idTask = int.Parse(Console.ReadLine()??"0");
                // Update the Dependency record with new user input
                s_bl!.Task.Update(newTask(idTask));
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
        Console.WriteLine("idTask:");
        int idTask = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("aliasTask:");
        string aliasTask = Console.ReadLine() ?? "";
        TaskInEngineer task= new TaskInEngineer() { Id=idTask, Alias=aliasTask };

        //creates new Engineer and returns it
        Engineer newEngineer = new Engineer() { Id=id, Name=name, Email=email, Cost=cost, level=level1, Task=task };
        return newEngineer;
    }

    private static BO.Task newTask(int id)
    {
        //user input
        Console.WriteLine("enter alias,description,if the task is mile stone=false, required effort time, created in date,scheduled date=null,start date, complete date, dead line, deliverables,remarks,engineer id, engineer's experience,complexity");
        string alias = Console.ReadLine() ?? "";
        string description = Console.ReadLine() ?? "";
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
            throw new BlWrongInputFormatException("input not in TimeSpan format");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime createdInDate))
            throw new BlWrongInputFormatException("input not in DateTime format");
        Status status = (Status)int.Parse(Console.ReadLine() ?? "0");
        List<TaskInList> listDep = new List<TaskInList>();
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
        if (DateTime.TryParse(Console.ReadLine(), out DateTime scheduledDate))
            throw new BlWrongInputFormatException("input not in DateTime format");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            throw new BlWrongInputFormatException("input not in DateTime format");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime forecastDate))
            throw new BlWrongInputFormatException("input not in DateTime format");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime completeDate))
            throw new BlWrongInputFormatException("input not in DateTime format");
        Console.WriteLine("enter num of dependency, to finish enter -1");
        string deliverables = Console.ReadLine() ?? "";
        string remarks = Console.ReadLine() ?? "";
        int engineerId = int.Parse(Console.ReadLine() ?? "");
        string engineerName = Console.ReadLine() ?? "";
        EngineerInTask engineer= new EngineerInTask() { Id=engineerId, Name=engineerName };
        EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        //create new Task and returns it
        BO.Task newTask = new BO.Task() {Id=id, Alias=alias, Description=description, RequiredEffortTime=requiredEffortTime, ScheduledDate=scheduledDate, CreatedAtDate=createdInDate,Status=status ,Dependencies=listDep , StartDate=startDate,ForecastDate=forecastDate ,CompleteDate=completeDate,Deliverables=deliverables, Remarks=remarks,Engineer=engineer ,Complexity=complexity, };  
        return newTask;
    }
    // dependencies 
    private static void createSubMenu(MainMenu entity)
    {
    }

    private static void deleteSubMenu(MainMenu entity)
    {
    }

}
