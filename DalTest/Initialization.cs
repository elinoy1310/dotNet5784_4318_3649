
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



