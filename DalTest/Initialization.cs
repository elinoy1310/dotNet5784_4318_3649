
namespace DalTest;

using System.Security.Cryptography;
using DalApi;
using DO;
public static class Initialization
{
    private static IEngineer? s_dalEngineer; 
    private static ITask? s_dalTask; 
    private static IDependency? s_dalDependency; 

    private static readonly Random s_rand = new();




    
    //צריך לכתוב פה פונקציה נורמלית
    private static void createTasks()
    {
        DateTime projectStartDate = new DateTime(2024, 4, 8), scheduledFinishedDate=new DateTime(2024,9,1);
        string[] alias =
            {
                "Project Inception",
                "Feasibility Study",
                "Requirement Analysis",
                "System Design",
                "Technology Stack Selection",
                "Prototyping",
                "Database Design",
                "Frontend Development",
                "Backend Development",
                "API Development",
                "Testing Environment Setup",
                "Unit Testing",
                "Integration Testing",
                "System Testing",
                "User Acceptance Testing (UAT)",
                "Bug Fixing and Optimization",
                "Documentation",
                "Deployment Preparation",
                "Deployment and Release",
                "Post-Implementation Review"
                ,"T1","T2","T3","T4","T5"
              };
        string[] descriptions =
            { 
                "Define the project scope, objectives, and requirements through discussions with stakeholders and clients."
                ,"Conduct a feasibility study to assess the technical, economic, and operational aspects of the project."
                ,"Gather and document detailed requirements, including functional and non-functional specifications."
                ,"Create a comprehensive system design, including architecture, data models, and interface specifications."
                ,"Choose the appropriate technologies and frameworks based on project requirements and constraints."
                ,"Develop a prototype or proof of concept to validate key functionalities and design decisions."
                ,"Design the database schema, relationships, and data flow for efficient data storage and retrieval."
                ,"Begin developing the user interface (UI) based on the approved designs and wireframes."
                ,"Implement server-side logic, business rules, and integration points according to the system design."
                ,"Create application programming interfaces (APIs) for seamless communication between frontend and backend components."
                ,"Configure testing environments, including unit testing, integration testing, and system testing environments."
                ,"Conduct unit tests to ensure individual components and functions meet the specified requirements."
                ,"Perform integration testing to validate the collaboration and functionality of integrated system components."
                ,"Execute comprehensive system tests to verify the end-to-end functionality and performance of the entire system."
                ,"Collaborate with end-users to conduct UAT and ensure the software meets user expectations."
                ,"Address and resolve any identified issues, bugs, or performance bottlenecks from testing phases."
                ,"Document the codebase, APIs, and system architecture, providing comprehensive information for future reference."
                ,"Prepare for the deployment phase by finalizing configurations, setting up production environments, and creating deployment scripts."
                ,"Deploy the software to the production environment and release it to end-users following a well-defined deployment plan."
                ,"Conduct a post-implementation review to assess the project's success, identify lessons learned, and gather feedback for future improvements."
                ,"Test stage 1","Test stage 2","Test stage 3","Test stage 4","Test stage 5"
        };
        int month=scheduledFinishedDate.Month;
        int j = 20;
        
        for(int i=0; i<20; i++)
        {
            int numOfDays = s_rand.Next(3, 21);            
            DateTime temp=new DateTime(2024,month,numOfDays);
            TimeSpan ts =DateTime.Now-temp;//created in date
            month=month-numOfDays==projectStartDate.Month?month:month-numOfDays;
            //  month += 2;
            



            s_dalTask!.Create(new Task() { Alias = alias[i], Description = descriptions[i],CreatedInDate=Convert.ToDateTime(ts),ScheduledDate= new DateTime(2024,4,j-i) /*deadLine,complexity*/ });
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
            Dependency newDep = new Dependency() { Dependent = _dependent, DependsOnTask = _dependsOnTask }; // Create a new Dependency object
            s_dalDependency!.Create(newDep);     // Add the new dependency to the data access layer
        }
    }
}



