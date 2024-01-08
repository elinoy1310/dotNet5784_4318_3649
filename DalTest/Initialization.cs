
namespace DalTest;

using System.Security.Cryptography;
using DalApi;
using DO;
public static class Initialization
{
    private static IEngineer? s_dalEngineer; 
    private static ITask? s_dalTask; 
    private static IDependency? s_dalDependency; 

    private static readonly Random s_rand = new Random(DateTime.Now.Millisecond);


    public static void Do(IEngineer? dalEngineer, ITask? dalTask, IDependency? dalDependency) 
    {
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        createEngineer();
        createTasks();
        createDependency();
    }


    /// <summary>
    /// creates and adds tasks to the system with random data.
    /// </summary>
    private static void createTasks()
    {
        //determining the project time
        DateTime projectStartDate = new DateTime(2024, 4, 8), scheduledFinishedDate=new DateTime(2024,9,1);
        TimeSpan timeForTheProject = scheduledFinishedDate.Subtract(projectStartDate);

        //creates tasks definitions, so alias[i] corresponds to description[i]
        string[] alias =
            {
                "Project Inception",
                "Feasibility Study",
                "Requirement Analysis",
                "System Design",
                "TSS",
                "Prototyping",
                "Database Design",
                "Frontend Development",
                "Backend Development",
                "API Development",
                "TES",
                "Unit Testing",
                "Integration Testing",
                "System Testing",
                "UAT",
                "BFO",
                "Documentation",
                "Deployment Preparation",
                "DR",
                "PIR"
                ,"T1","T2","T3","T4","T5"
              };
        string[] descriptions =
            { 
                "Define the project scope, objectives, and requirements through discussions with stakeholders and clients."
                ,"Conduct a feasibility study to assess the technical, economic, and operational aspects of the project."
                ,"Gather and document detailed requirements, including functional and non-functional specifications."
                ,"Create a comprehensive system design, including architecture, data models, and interface specifications."
                ,"Technology Stack Selection=Choose the appropriate technologies and frameworks based on project requirements and constraints."
                ,"Develop a prototype or proof of concept to validate key functionalities and design decisions."
                ,"Design the database schema, relationships, and data flow for efficient data storage and retrieval."
                ,"Begin developing the user interface (UI) based on the approved designs and wireframes."
                ,"Implement server-side logic, business rules, and integration points according to the system design."
                ,"Create application programming interfaces (APIs) for seamless communication between frontend and backend components."
                ,"Testing Environment Setup=Configure testing environments, including unit testing, integration testing, and system testing environments."
                ,"Conduct unit tests to ensure individual components and functions meet the specified requirements."
                ,"Perform integration testing to validate the collaboration and functionality of integrated system components."
                ,"Execute comprehensive system tests to verify the end-to-end functionality and performance of the entire system."
                ,"User Acceptance Testing =Collaborate with end-users to conduct UAT and ensure the software meets user expectations."
                ,"Bug Fixing and Optimization=Address and resolve any identified issues, bugs, or performance bottlenecks from testing phases."
                ,"Document the codebase, APIs, and system architecture, providing comprehensive information for future reference."
                ,"Prepare for the deployment phase by finalizing configurations, setting up production environments, and creating deployment scripts."
                ,"Deployment and Release=Deploy the software to the production environment and release it to end-users following a well-defined deployment plan."
                ,"Post-Implementation Review=Conduct a post-implementation review to assess the project's success, identify lessons learned, and gather feedback for future improvements."
                ,"Test stage 1","Test stage 2","Test stage 3","Test stage 4","Test stage 5"
        };

        int days, month;
        DateTime createdAt, sceduledStart,deadLine;
        TimeSpan requirdEffortTime; 
     

        for (int i=0; i<20; i++)
        {
            //selecting month+days randomly (with certain restrictions)
            days = s_rand.Next(1, 31); 
            month = s_rand.Next(1, projectStartDate.Month);
            month= month<DateTime.Now.Month? month : DateTime.Now.Month;
            if(month==0)//if now.month=1 and month also 1, from the previous line month=0
                days = s_rand.Next(1, DateTime.Now.Day);  
            
            //date selection for certain dates thar required for each task
            createdAt=new DateTime(2024,month,days);//the task was created after 1.1.2024
            sceduledStart = projectStartDate.AddDays(i);//scheduled date for starting the task
            requirdEffortTime = timeForTheProject / (alias.Length - i);//divides the time of the planned tasks into periods of time
            deadLine=sceduledStart.AddDays(requirdEffortTime.Days);
            deadLine=deadLine<scheduledFinishedDate?deadLine:scheduledFinishedDate; //deadline won't be after the project end date

            //selecting comlexity randomly
            int randomComplexity =s_rand.Next(0, 5);
            EngineerExperience taskComplexity = (EngineerExperience)randomComplexity;
     
            Task newTask=new Task() { Alias = alias[i], Description = descriptions[i], CreatedInDate = createdAt, ScheduledDate = sceduledStart,RequiredEffortTime= requirdEffortTime, Deadline = deadLine, Complexity = taskComplexity };

            s_dalTask!.Create(newTask);// add the new task to the data list(tasks)
        }
    }

    /// <summary>
    ///  Creates and adds engineers to the system with random data.
    /// </summary>
    private static void createEngineer()
    {
        // Array of engineer names
        string[] EngineerNames =
        {
        "Hadar Nagar", "Elinoy Damari", "Yair Cohen",
        "Shira Mazuz", "Ayala Lin", "David Levi"
        };
        
        for (int i=0;i<6;i++) // Loop to create engineers
        {
            int _id = s_rand.Next(200000000, 400000000);  // Generate a random ID between 200,000,000 and 400,000,000
            string? _email = EngineerNames[i].Replace(" ", "") + "@gmail.com"; // Generate email by removing spaces and adding "@gmail.com" 
            double _cost = s_rand.Next(200, 600);  // Generate a random cost between 200 and 600
            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 4);  // Generate a random EngineerExperience level
            Engineer newEng = new(_id, _email, _cost, EngineerNames[i], _level);  // Create a new Engineer object
            s_dalEngineer!.Create(newEng);  // Add the new engineer to the data access layer
        }

    }
    /// <summary>
    ///  Creates and adds dependencies to the system with random data.
    /// </summary>
    private static void createDependency() 
    {
        for (int i=0;i<40;i++) // Loop to create dependencies
        {
            // Generate a random dependent and dependsOnTask values
            int _dependent = s_rand.Next(1, 20); 
            int _dependsOnTask = s_rand.Next(1, 20);
            if (_dependent == _dependsOnTask)
            {
                if (_dependent < 20)
                    _dependent++;
                if (_dependent > 0)
                    _dependent--;
            }
                    Dependency newDep = new Dependency() { Dependent = _dependent, DependsOnTask = _dependsOnTask }; // Create a new Dependency object
            s_dalDependency!.Create(newDep);     // Add the new dependency to the data access layer
        }
    }


}



