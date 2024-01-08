using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask=new TaskImplementation();
        private static IEngineer? s_dalEndineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEndineer,s_dalTask,s_dalDependency);

            }

            catch(Exception ex) 
            {

                Console.WriteLine(ex.Message);
            }
            
        }
    }
}