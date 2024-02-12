
namespace DalTest;

using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using DalApi;
using DO;
using Microsoft.VisualBasic;

using static System.Net.Mime.MediaTypeNames;

public static class Initialization
{
    private static IDal? s_dal;

    private static readonly Random s_rand = new Random(DateTime.Now.Millisecond);
    public static void Do() 
    {
        s_dal = DalApi.Factory.Get;
        createEngineer();
        createTasks();
        createDependency();
    }


    /// <summary>
    /// creates and adds tasks to the system with random data.
    /// </summary>
    private static void createTasks()
    {
        //creates tasks definitions, so alias[i] corresponds to description[i]
        string[] alias =
        {
            "c.c.w.r.",
            "s.a.d.",
            "c.s.a.s.",
            "r.a.",
            "r.p.",
            "b.p.",
            "p.s.",
            "s.a.w.d.",
            "p.d.",
            "d.d.",
            "e.e.a.",
            "d.o.a.s.p",
            "c.w.o.a.",
            "i.p.",
            "q.a.p.",
            "t.p.",
            "c.d.",
            "c.c.p",
            "t.p.d",
            "o.s."
               
        };
        string[] descriptions =
        {
            "(Checking compliance with regulations),Investigating and ensuring compliance with relevant electrical standards and regulations.",
            "(System architecture design) Development of system architecture and high-level schemes.",
            "(Component selection and specification) Selection and specification of electrical components according to project requirements.",
            "(Risk assessment) Identifying potential risks.",
            "(Reduction plan) Developing a mitigation plan to address potential risks.",
            "(Budget planning) Creating a detailed budget, estimating costs for materials, labor, and equipment.",
            "(Procurement strategy) Planning and implementing a strategy for purchasing necessary components, taking into account delivery times and suppliers.",
            "(Schematic and wiring diagrams) Creating detailed schematics and wiring diagrams for the electrical system.",
            "(Prototype development) Developing prototypes for critical components and performing tests to verify functionality.",
            "(Detailed design) Creating a detailed electrical design, specifying connections, cable routes, and control logic.",
            "(Energy efficiency analysis) Analysis and optimization of the energy efficiency of the planned electrical system.",
            "(Development of a safety plan) Development of a comprehensive safety plan for the installation and operation of the electrical system.",
            "(Coordination with other areas) Coordination with professionals from other disciplines to ensure seamless integration of electrical systems.",
            "(Installation plan) Planning the installation process, including sequence, logistics and coordination with the installation teams.",
            "(Quality assurance protocol) Development and implementation of a quality assurance protocol to ensure compliance with design specifications.",
            "(Test procedures) Development of detailed test procedures for different phases of the project, including acceptance tests.",
            "(Create documentation) Creating comprehensive documentation, including operating manuals, maintenance manuals, and as-built drawings.",
            "(Customer communication plan) Creating a plan for ongoing communication with the client, including project updates and milestone reviews.",
            "(Training program development) Developing a training program for maintenance personnel to ensure they can operate the system effectively.",
            "(Order supervision) Supervising the start-up phase, making sure that all the components work correctly and meet the specifications."
    
        };

        int days, month;
        DateTime createdAt;
        for (int i=0; i<20; i++)
        {
            //selecting month+days randomly (with certain restrictions)
            TimeSpan requiredEffortTime = new TimeSpan(s_rand.Next(0,3), s_rand.Next(1, 10), 0,0);
            days = s_rand.Next(1, 31); 
            month = s_rand.Next(1, DateTime.Now.Month+1);
            month= month<DateTime.Now.Month? month : DateTime.Now.Month-1;
            if(month==0)//if now.month=1 and month also 1, from the previous line month=0
            {
                month = 1;
                days = s_rand.Next(1, DateTime.Now.Day);  
            }
                          
            //date selection for certain dates thar required for each task
            createdAt=new DateTime(DateTime.Now.Year,month,days);//the task was created after 1.1.this_year and before today's date         

            //selecting comlexity randomly
            int randomComplexity =s_rand.Next(0, 5);
            EngineerExperience taskComplexity = (EngineerExperience)randomComplexity;

            //initialize only alias,description,createdInDate,complexity and the rest fields are filled with default values
            Task newTask = new Task() with { Alias = alias[i], Description = descriptions[i], CreatedInDate = createdAt, Complexity = taskComplexity, RequiredEffortTime = requiredEffortTime };

            s_dal!.Task.Create(newTask);// add the new task to the data list(tasks)
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
            s_dal!.Engineer.Create(newEng);  // Add the new engineer to the data access layer
        }

    }
    /// <summary>
    ///  Creates and adds dependencies to the system 
    /// </summary>
    private static void createDependency() 
    {
        s_dal!.Dependency.Create(new Dependency(0,1,2));
        s_dal!.Dependency.Create(new Dependency(0,1,3));
        s_dal!.Dependency.Create(new Dependency(0,1,4));
        s_dal!.Dependency.Create(new Dependency(0,1,5));
        s_dal!.Dependency.Create(new Dependency(0,2,3));
        s_dal!.Dependency.Create(new Dependency(0,2,4));
        s_dal!.Dependency.Create(new Dependency(0,2,5));
        s_dal!.Dependency.Create(new Dependency(0,3,4));
        s_dal!.Dependency.Create(new Dependency(0,3,5));
        s_dal!.Dependency.Create(new Dependency(0,3,6));
        s_dal!.Dependency.Create(new Dependency(0,4,5));
        s_dal!.Dependency.Create(new Dependency(0,4,6));
        s_dal!.Dependency.Create(new Dependency(0,4,7));
        s_dal!.Dependency.Create(new Dependency(0,5,6));
        s_dal!.Dependency.Create(new Dependency(0,5,7));
        s_dal!.Dependency.Create(new Dependency(0,5,8));
        s_dal!.Dependency.Create(new Dependency(0,6,7));
        s_dal!.Dependency.Create(new Dependency(0,6,8));
        s_dal!.Dependency.Create(new Dependency(0,6,9));
        s_dal!.Dependency.Create(new Dependency(0,7,8));
        s_dal!.Dependency.Create(new Dependency(0,7,9));
        s_dal!.Dependency.Create(new Dependency(0,7,10));
        s_dal!.Dependency.Create(new Dependency(0,8,9));
        s_dal!.Dependency.Create(new Dependency(0,8,10));
        s_dal!.Dependency.Create(new Dependency(0,8,11));
        s_dal!.Dependency.Create(new Dependency(0,9,10));
        s_dal!.Dependency.Create(new Dependency(0,9,11));
        s_dal!.Dependency.Create(new Dependency(0,9,12));
        s_dal!.Dependency.Create(new Dependency(0,10,11));
        s_dal!.Dependency.Create(new Dependency(0,10,12));
        s_dal!.Dependency.Create(new Dependency(0,10,13));
        s_dal!.Dependency.Create(new Dependency(0,11,12));
        s_dal!.Dependency.Create(new Dependency(0,11,13));
        s_dal!.Dependency.Create(new Dependency(0,12,13));
        s_dal!.Dependency.Create(new Dependency(0,17,19));
        s_dal!.Dependency.Create(new Dependency(0,17,20));
        s_dal!.Dependency.Create(new Dependency(0,1,17));
        s_dal!.Dependency.Create(new Dependency(0,18,19));
        s_dal!.Dependency.Create(new Dependency(0,18,20));
        s_dal!.Dependency.Create(new Dependency(0,1,20));

    }
}



