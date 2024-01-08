
namespace DalTest;
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
}
